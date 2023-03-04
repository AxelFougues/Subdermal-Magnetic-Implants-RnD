using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour{

    public Vector3 axis = new Vector3(1,0,0);

    SMIS smis;

    IEnumerator coroutine;
    private void OnEnable() {
        smis = GetComponent<SMIS>();
#if UNITY_ANDROID
        Input.compass.enabled = true;
        coroutine = CompasFeedback();
        StartCoroutine(coroutine);
#endif
    }

    IEnumerator CompasFeedback() {
        while (true) {
            Vector3 value = Vector3.Scale(Input.compass.rawVector, axis);
            smis.doDirectFeedback(0, mapValue(value.magnitude, -100, 100, 0, 1));
            smis.doDirectFeedback(1, mapValue(value.magnitude, -100, 100, 0, 1));
            yield return new WaitForFixedUpdate();
        }
    }


    private void OnDisable() {
        if (coroutine != null) StopCoroutine(coroutine);
    }


    float mapValue(float referenceValue, float fromMin, float fromMax, float toMin, float toMax) {
        /* This function maps (converts) a Float value from one range to another */
        return toMin + (referenceValue - fromMin) * (toMax - toMin) / (fromMax - fromMin);
    }

}
