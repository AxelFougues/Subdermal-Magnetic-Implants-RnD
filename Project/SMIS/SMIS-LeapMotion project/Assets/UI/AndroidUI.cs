using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidUI : MonoBehaviour{

    public GameObject smis;

    public void startBT() {
        smis.GetComponent<BTSensor>().enabled = true;
    }

    public void stopBT() {
        smis.GetComponent<BTSensor>().enabled = false;
    }

    public void startCompass() {
        smis.GetComponent<Compass>().enabled = true;
    }

    public void stopCompass() {
        smis.GetComponent<Compass>().enabled = false;
    }
}
