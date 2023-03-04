#include <SoftwareSerial.h>
SoftwareSerial BTserial(10, 11); // RX | TX
const int pingPin = 12; // Trigger Pin of Ultrasonic Sensor
const int echoPin = 13; // Echo Pin of Ultrasonic Sensor

//OUTPUT
const float RANGE_MIN = 0;
const float RANGE_MAX = 1000;
float lastOutValue = 0;

//SENSOR
int sensorType = 0;
void none();        //0 - Standby
void test();        //1 - Test
void ultrasound();  //2 - Ultrasound
typedef void (*SensorFunction)();
SensorFunction sensorFunctions[] = {
  &none,
  &test,
  &ultrasound
  };

void setup() {
  pinMode(pingPin, OUTPUT);
  pinMode(echoPin, INPUT);
  BTserial.begin(9600);
}

void loop() {
  if (BTserial.available() && (char)BTserial.read() == '@') {
    sensorType = (char)BTserial.read() - '0';
  }
  sensorFunctions[sensorType]();
}


//######################################Sensors
void none(){
  delay(100);
}

void test(){
  BTserial.println("0 - Standby");
  BTserial.println("1 - Test");
  BTserial.println("2 - Ultrasound");
  sensorType = 0;
}

void ultrasound() {
  long duration, cm;
  digitalWrite(pingPin, LOW);
  delayMicroseconds(2);
  digitalWrite(pingPin, HIGH);
  delayMicroseconds(10);
  digitalWrite(pingPin, LOW);
  duration = pulseIn(echoPin, HIGH);
  cm = constrain(microsecondsToCentimeters(duration), 0.0, 400.0);
  lastOutValue = mapValue(cm, 0, 400, RANGE_MIN, RANGE_MAX);
  sendBT();
}

//#####################################Utilities

long microsecondsToCentimeters(long microseconds) {
  return microseconds / 29 / 2;
}

float mapValue(float referenceValue, float fromMin, float fromMax, float toMin, float toMax) {
  /* This function maps (converts) a Float value from one range to another */
  return toMin + (referenceValue - fromMin) * (toMax - toMin) / (fromMax - fromMin);
}

float sendBT() {
  BTserial.println(lastOutValue);
  delay(100);
}
