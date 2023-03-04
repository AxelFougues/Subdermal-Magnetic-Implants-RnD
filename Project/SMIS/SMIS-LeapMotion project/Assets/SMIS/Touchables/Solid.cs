using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Solid Touchable Material")]
public class Solid : TouchableMaterialProperties {

    [Tooltip("Texture frequency in mm")]
    [Range(0f, 10f)]
    public float spatialPeriod = 3f; //in mm
    [Tooltip("Arbitrary intensity of the texture, 0 means smooth")]
    [Range(0.0f, 60f)]
    public float softness = 0.5f; //in mm
    public AnimationCurve pressureResponse;
    public float baseFrequency = 10.0f;


    public override float getStaticFrequency() {
        return baseFrequency;
    }

    public override float getPressureAmplitude(float penetrationDepth) {
        float maxDepth = pressureBuffer + softness;
        penetrationDepth = Mathf.Clamp(penetrationDepth, 0, maxDepth);
        if (pressureResponse.keys.Length > 0) return pressureResponse.Evaluate(penetrationDepth / maxDepth);
        else return mapValue(penetrationDepth, 0, maxDepth, 0, 1);
    }

    public override float getTextureFrequency( float sliding ) {
        if (spatialPeriod == 0) return 0;
        // mm/s / mm = s
        return Mathf.Clamp((sliding / spatialPeriod), 0, 400);
    }


}
