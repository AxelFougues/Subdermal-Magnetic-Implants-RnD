using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Liquid Touchable Material")]
public class Liquid : TouchableMaterialProperties{

    [Tooltip("Arbitrary viscosity of the liquid")]
    [Range(0.001f, 1f)]
    public float viscosity = 0.5f;

    public override float getStaticFrequency() {
        return mapValue(viscosity, 0, 1, 5, 10);
    }

    public override float getPressureAmplitude(float penetrationDepth) {
        return mapValue(penetrationDepth, 0, pressureBuffer, 0, 1);
    }

    public override float getTextureFrequency(float sliding ) {
        return Mathf.Clamp(sliding * viscosity, getStaticFrequency(), 400);
    }

}
