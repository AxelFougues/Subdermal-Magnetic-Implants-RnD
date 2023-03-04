using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class BTSensorBridge : MonoBehaviour{
    public string DeviceName = "MLT-BT05";
    public Text BLE_StatusText;
    public Text bluetoothStatusText;
    public Text lastDataText;
    [HideInInspector]
    public float lastData;
    [HideInInspector]
    public string BLE_Status;
    [HideInInspector]
    public string bluetoothStatus;

    enum States {
        None,
        Scan,
        Connect,
        Subscribe,
        Unsubscribe,
        Disconnect,
        Communication
    }

    public enum Sensors {
        Standby = 0,
        Test = 1,
        Ultrasound = 2
    }

    private string ServiceUUID = "FFE0";
    private string Characteristic = "FFE1";
    private bool _workingFoundDevice = true;
    private bool _connected = false;
    private float _timeout = 0f;
    private States _state = States.None;
    private bool _foundID = false;
    private string _hm10; // this is our hm10 device

    public void send(string text) {
        sendString(text);
    }

    public void setSensor(Sensors sensor) {
        sendString("@"+(int)sensor);
    }

    public void connect() {
        if (BLE_Status != "Subscribed") startProcess();
    }

    private void OnEnable() {
        startProcess();
    }

    void Update() {
        if (_timeout > 0f) {
            _timeout -= Time.deltaTime;
            if (_timeout <= 0f) {
                _timeout = 0f;

                switch (_state) {
                    case States.None:
                        break;

                    case States.Scan:
                        bluetoothStatus = "Scanning for sensors...";

                        BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) => {

                            // we only want to look at devices that have the name we are looking for
                            // this is the best way to filter out devices
                            if (name.Contains(DeviceName)) {
                                _workingFoundDevice = true;

                                // it is always a good idea to stop scanning while you connect to a device
                                // and get things set up
                                BluetoothLEHardwareInterface.StopScan();
                                bluetoothStatus = "";

                                // add it to the list and set to connect to it
                                _hm10 = address;

                                BLE_Status = "Sensor found";

                                setState(States.Connect, 0.5f);

                                _workingFoundDevice = false;
                            }

                        }, null, false, false);
                        break;

                    case States.Connect:
                        // set these flags
                        _foundID = false;

                        BLE_Status = "Connecting";

                        // note that the first parameter is the address, not the name. I have not fixed this because
                        // of backwards compatiblity.
                        // also note that I am note using the first 2 callbacks. If you are not looking for specific characteristics you can use one of
                        // the first 2, but keep in mind that the device will enumerate everything and so you will want to have a timeout
                        // large enough that it will be finished enumerating before you try to subscribe or do any other operations.
                        BluetoothLEHardwareInterface.ConnectToPeripheral(_hm10, null, null, (address, serviceUUID, characteristicUUID) => {

                            if (isEqual(serviceUUID, ServiceUUID)) {
                                // if we have found the characteristic that we are waiting for
                                // set the state. make sure there is enough timeout that if the
                                // device is still enumerating other characteristics it finishes
                                // before we try to subscribe
                                if (isEqual(characteristicUUID, Characteristic)) {
                                    _connected = true;
                                    setState(States.Subscribe, 2f);

                                    BLE_Status = "Connected";
                                }
                            }
                        }, (disconnectedAddress) => {
                            BluetoothLEHardwareInterface.Log("Device disconnected: " + disconnectedAddress);
                            BLE_Status = "Disconnected";
                        });
                        break;

                    case States.Subscribe:
                        BLE_Status = "Subscribing";

                        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(_hm10, ServiceUUID, Characteristic, null, (address, characteristicUUID, bytes) => {

                            float.TryParse(Encoding.UTF8.GetString(bytes), out lastData);
                        });

                        // set to the none state and the user can start sending and receiving data
                        _state = States.None;
                        BLE_Status = "Subscribed";
                        break;

                    case States.Unsubscribe:
                        BluetoothLEHardwareInterface.UnSubscribeCharacteristic(_hm10, ServiceUUID, Characteristic, null);
                        setState(States.Disconnect, 4f);
                        break;

                    case States.Disconnect:
                        if (_connected) {
                            BluetoothLEHardwareInterface.DisconnectPeripheral(_hm10, (address) => {
                                BluetoothLEHardwareInterface.DeInitialize(() => {

                                    _connected = false;
                                    _state = States.None;
                                });
                            });
                        } else {
                            BluetoothLEHardwareInterface.DeInitialize(() => {

                                _state = States.None;
                            });
                        }
                        break;
                }
            }
        }

        if (lastDataText != null) lastDataText.text = lastData.ToString();
        if (BLE_StatusText != null) BLE_StatusText.text = BLE_Status;
        if (bluetoothStatusText != null) bluetoothStatusText.text = bluetoothStatus;
    }

    void reset() {
        _workingFoundDevice = false;    // used to guard against trying to connect to a second device while still connecting to the first
        _connected = false;
        _timeout = 0f;
        _state = States.None;
        _foundID = false;
        _hm10 = null;
    }

    void setState(States newState, float timeout) {
        _state = newState;
        _timeout = timeout;
    }

    void startProcess() {
        BLE_Status = "Starting Process";
        bluetoothStatus = "Initializing...";

        reset();
        BluetoothLEHardwareInterface.Initialize(true, false, () => {

            setState(States.Scan, 0.1f);
            bluetoothStatus = "Initialized";

        }, (error) => {

            BluetoothLEHardwareInterface.Log("Error: " + error);
        });
    }

    string fullUUID(string uuid) {
        return "0000" + uuid + "-0000-1000-8000-00805F9B34FB";
    }

    bool isEqual(string uuid1, string uuid2) {
        if (uuid1.Length == 4)
            uuid1 = fullUUID(uuid1);
        if (uuid2.Length == 4)
            uuid2 = fullUUID(uuid2);

        return (uuid1.ToUpper().Equals(uuid2.ToUpper()));
    }

    void sendString(string value) {
        var data = Encoding.UTF8.GetBytes(value);
        // notice that the 6th parameter is false. this is because the HM10 doesn't support withResponse writing to its characteristic.
        // some devices do support this setting and it is prefered when they do so that you can know for sure the data was received by 
        // the device
        BluetoothLEHardwareInterface.WriteCharacteristic(_hm10, ServiceUUID, Characteristic, data, data.Length, false, (characteristicUUID) => {

            BluetoothLEHardwareInterface.Log("Write Succeeded");
        });
    }

    void sendByte(byte value) {
        byte[] data = new byte[] { value };
        // notice that the 6th parameter is false. this is because the HM10 doesn't support withResponse writing to its characteristic.
        // some devices do support this setting and it is prefered when they do so that you can know for sure the data was received by 
        // the device
        BluetoothLEHardwareInterface.WriteCharacteristic(_hm10, ServiceUUID, Characteristic, data, data.Length, false, (characteristicUUID) => {

            BluetoothLEHardwareInterface.Log("Write Succeeded");
        });
    }

}
