using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSensor : MonoBehaviour{

    public BTSensorBridge.Sensors sensorType;
    public bool filterValues = true;

    BTSensorBridge bt;
    SMIS smis;

    RollingAverageFilter sensorFilter = new RollingAverageFilter(5, 1);

    IEnumerator coroutine;

    private void OnEnable() {
        smis = GetComponent<SMIS>();
        bt = GetComponent<BTSensorBridge>();
#if UNITY_ANDROID
        coroutine = SensorFeedback(sensorType);
        StartCoroutine(coroutine);
#endif
    }


    IEnumerator SensorFeedback(BTSensorBridge.Sensors sensor) {
        bt.enabled = true;
        bt.connect();
        sensorFilter.clear();
        while (bt.BLE_Status != "Subscribed") {
            Debug.Log("Waiting for bluetooth sensor to connect");
            yield return new WaitForSeconds(1);
        }
        bt.setSensor(sensor);
        while (true) {
            if (bt.lastData >= 0) {
                float value;
                if (filterValues) value = sensorFilter.getValue(bt.lastData);
                else value = bt.lastData;
                smis.doDirectFeedback(0, mapValue(value, 0, 1000, 0, 1));
                smis.doDirectFeedback(1, mapValue(value, 0, 1000, 0, 1));
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnDisable() {
        if(coroutine != null) StopCoroutine(coroutine);
    }


    float mapValue(float referenceValue, float fromMin, float fromMax, float toMin, float toMax) {
        /* This function maps (converts) a Float value from one range to another */
        return toMin + (referenceValue - fromMin) * (toMax - toMin) / (fromMax - fromMin);
    }
}
