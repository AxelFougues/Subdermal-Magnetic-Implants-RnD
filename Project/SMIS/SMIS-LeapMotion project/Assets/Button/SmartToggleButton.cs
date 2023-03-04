using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartToggleButton : SmartButton{
    public AudioClip activateOffAudio;
    public AudioClip deactivateOffAudio;

    public float onHeightOffset;

    protected float realOnHeightOffset;

    protected new void Start() {
        to = GetComponent<TouchableObject>();
        initialPos = actuator.transform.position;
        realFixedSpring = percentageDepthToValue(fixedSpring);
        realTriggerDepth = percentageDepthToValue(triggerDepth);
        realUntriggerDepth = percentageDepthToValue(untriggerDepth);
        realOnHeightOffset = percentageDepthToValue(onHeightOffset);
    }

    protected new void FixedUpdate() {
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
                    if (doAnimation) setActuatorDepth(calculatedDepth);

                    if (calculatedDepth >= realTriggerDepth) { //Beyond trigger point
                        if (doAudio && on) smis.doAudioFeedback(pressingFinger, activateAudio);
                        else if (doAudio) smis.doAudioFeedback(pressingFinger, activateOffAudio);
                        on = !on;
                        initialDepthOffset = on ? realOnHeightOffset : 0;
                        state = "triggered";
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
                        if (doAudio && on) smis.doAudioFeedback(pressingFinger, deactivateAudio);
                        else if (doAudio) smis.doAudioFeedback(pressingFinger, deactivateOffAudio);
                        state = "untriggered";
                    }
                } else {//Is not being touched anymore
                    state = "free";
                }
                break;

        }
    }

}
