using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TouchableObject))]
public class SmartButton : MonoBehaviour {

    public GameObject actuator;

    public bool on = false;

    public bool doAudio = true;

    public bool doAnimation = true;

    public float fixedSpring = 5f;

    public float triggerDepth = 90f;
    public float untriggerDepth = 75f;

    public AudioClip activateAudio;
    public AudioClip deactivateAudio;

    protected float realMaxDepth;
    protected float realFixedSpring;
    protected float realTriggerDepth;
    protected float realUntriggerDepth;


    protected int pressingFinger = 0; 
    protected string state = "free";
    protected float currentDepth = 0;
    protected float initialDepthOffset = 0;
    protected Vector3 initialPos;
    protected TouchableObject to;

    protected SMIS smis;

    protected void Awake() {
        smis = FindObjectOfType<SMIS>();
        realMaxDepth = (actuator.GetComponent<MeshRenderer>().bounds.size.y / 2) * 0.5f;
    }

    protected void Start(){
        to = GetComponent<TouchableObject>();
        initialPos = actuator.transform.position;
        realFixedSpring = percentageDepthToValue(fixedSpring);
        realTriggerDepth = percentageDepthToValue(triggerDepth);
        realUntriggerDepth = percentageDepthToValue(untriggerDepth);
    }

    protected void FixedUpdate(){
        switch (state) {
            case "free":
                if (to.beingTouched) state = "untriggered"; //Starts being pressed
                else if (currentDepth > initialDepthOffset) setActuatorDepth(currentDepth - realFixedSpring);
                break;

            case "untriggered":
                if (to.beingTouched) {//Is being pressed
                    float calculatedDepth = 0;
                    for (int i = 0; i < to.contactParameters.Count; i++) {
                        if (to.contactParameters[i].pDistance / 1000 > calculatedDepth) {
                            calculatedDepth = to.contactParameters[i].pDistance / 1000;
                            pressingFinger = i;
                        }
                    }
                    if(doAnimation) setActuatorDepth(calculatedDepth);

                    if (calculatedDepth >= realTriggerDepth) { //Beyond trigger point
                        if(doAudio) smis.doAudioFeedback(pressingFinger, activateAudio);
                        state = "triggered";
                        on = true;
                    }
                } else {//Is not being touched anymore
                    state = "free";
                }
                break;

            case "triggered":
                if (to.beingTouched) {//Is being pressed
                    float calculatedDepth = 0;
                    for (int i = 0; i < to.contactParameters.Count; i++) {
                        if (to.contactParameters[i].pDistance / 1000 > calculatedDepth) {
                            calculatedDepth = to.contactParameters[i].pDistance / 1000;
                            pressingFinger = i;
                        }
                    }
                    if (doAnimation) setActuatorDepth(calculatedDepth);

                    if (calculatedDepth <= realUntriggerDepth) { //Beyond untrigger point
                        if (doAudio) smis.doAudioFeedback(pressingFinger, deactivateAudio);
                        state = "untriggered";
                        on = false;
                    }
                } else {//Is not being touched anymore
                    state = "free";
                }
                break;

        }

    }

    protected void setActuatorDepth(float depth) {
        if (depth > realMaxDepth) depth = realMaxDepth;         
        if (depth < initialDepthOffset) depth = initialDepthOffset;
        currentDepth = depth;
        actuator.transform.position = initialPos - (actuator.transform.up * depth);
    }

    protected float percentageDepthToValue(float percentage) {
        return mapValue(percentage, 0, 100, 0, realMaxDepth);
    }

    float mapValue(float referenceValue, float fromMin, float fromMax, float toMin, float toMax) {
        /* This function maps (converts) a Float value from one range to another */
        return toMin + (referenceValue - fromMin) * (toMax - toMin) / (fromMax - fromMin);
    }

}
