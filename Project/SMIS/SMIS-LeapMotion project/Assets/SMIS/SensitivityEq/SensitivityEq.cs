using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SensitivityEq : MonoBehaviour{
    static AnimationCurve sineSensitivity = new AnimationCurve();
    static AnimationCurve sawSensitivity = new AnimationCurve();
    static AnimationCurve squareSensitivity = new AnimationCurve();

    static float filterStrength = 0.2f;

    private void Start() {
        //Default values as estimated from Axel's experimentation
        sineSensitivity.AddKey(10, 0.4206257242f);
        sineSensitivity.AddKey(20, 0.3626882966f);
        sineSensitivity.AddKey(30, 0.281575898f);
        sineSensitivity.AddKey(40, 0.1703360371f);
        sineSensitivity.AddKey(50, 0.07879490151f);
        sineSensitivity.AddKey(75, 0.01158748552f);
        sineSensitivity.AddKey(100, 0f);
        sineSensitivity.AddKey(125, 0.005793742758f);
        sineSensitivity.AddKey(150, 0.01158748552f);
        sineSensitivity.AddKey(200, 0.04287369641f);
        sineSensitivity.AddKey(250, 0.09443800695f);
        sineSensitivity.AddKey(300, 0.09420625724f);
        sineSensitivity.AddKey(350, 0.3383545771f);
        sineSensitivity.AddKey(400, 1f);

        sawSensitivity.AddKey(10, 0.006009615385f);
        sawSensitivity.AddKey(20, 0.006009615385f);
        sawSensitivity.AddKey(30, 0.01923076923f);
        sawSensitivity.AddKey(40, 0f);
        sawSensitivity.AddKey(50, 0.01081730769f);
        sawSensitivity.AddKey(75, 0.009615384615f);
        sawSensitivity.AddKey(100, 0.03125f);
        sawSensitivity.AddKey(125, 0.03245192308f);
        sawSensitivity.AddKey(150, 0.04447115385f);
        sawSensitivity.AddKey(200, 0.09495192308f);
        sawSensitivity.AddKey(250, 0.1358173077f);
        sawSensitivity.AddKey(300, 0.2584134615f);
        sawSensitivity.AddKey(350, 0.6394230769f);
        sawSensitivity.AddKey(400, 1f);

        squareSensitivity.AddKey(10, 0.1501597444f);
        squareSensitivity.AddKey(20, 0.08466453674f);
        squareSensitivity.AddKey(30, 0.06549520767f);
        squareSensitivity.AddKey(40, 0.06709265176f);
        squareSensitivity.AddKey(50, 0.04792332268f);
        squareSensitivity.AddKey(75, 0f);
        squareSensitivity.AddKey(100, 0f);
        squareSensitivity.AddKey(125, 0.01437699681f);
        squareSensitivity.AddKey(150, 0.0303514377f);
        squareSensitivity.AddKey(200, 0.1118210863f);
        squareSensitivity.AddKey(250, 0.1373801917f);
        squareSensitivity.AddKey(300, 0.2396166134f);
        squareSensitivity.AddKey(350, 0.7587859425f);
        squareSensitivity.AddKey(400, 1f);
    }

    public static float equalizeSineAmplitude(float amplitude, float frequency) {
        if (amplitude == 0) return 0;
        float multiplier = sineSensitivity.Evaluate(frequency) * filterStrength + (1 - filterStrength) / 2;
        return amplitude * multiplier;
    }

    public static float equalizeSawAmplitude(float amplitude, float frequency) {
        if (amplitude == 0) return 0;
        return (amplitude + sawSensitivity.Evaluate(frequency) * filterStrength) / (filterStrength + 1);
    }

    public static float equalizeSquareAmplitude(float amplitude, float frequency) {
        if (amplitude == 0) return 0;
        return (amplitude + squareSensitivity.Evaluate(frequency) * filterStrength) / (filterStrength + 1);
    }
}
