using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class TouchableMaterialProperties : ScriptableObject {

    //Default fixed minimum surface thickness to mimic pressure in mm.
    protected const float pressureBuffer = 10f;

    public abstract float getStaticFrequency();
    public abstract float getPressureAmplitude(float penetrationDepth);
    public abstract float getTextureFrequency(float sliding );

    protected float mapValue(float referenceValue, float fromMin, float fromMax, float toMin, float toMax) {
        /* This function maps (converts) a Float value from one range to another */
        return toMin + (referenceValue - fromMin) * (toMax - toMin) / (fromMax - fromMin);
    }
}
