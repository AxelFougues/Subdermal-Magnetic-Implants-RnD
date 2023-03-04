using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experiment : MonoBehaviour{

    public bool track = true;
    public bool displayTracking = true;

    bool started = false;
    Spawner spawner;
    ExperimentLoggerManager selm;

    private void Start() {
        spawner = GetComponent<Spawner>();
        selm = GetComponent<ExperimentLoggerManager>();
    }

    void Update(){
        if (!started && Input.GetKeyUp(KeyCode.Space)) {
            spawner.spawn();
            if(track)selm.start();
            started = true;
        }else if (started && Input.GetKeyUp(KeyCode.Space)) {
            if (track) selm.stop();
            if (track && displayTracking) selm.displayLast();
            started = false;
        }
    }
}
