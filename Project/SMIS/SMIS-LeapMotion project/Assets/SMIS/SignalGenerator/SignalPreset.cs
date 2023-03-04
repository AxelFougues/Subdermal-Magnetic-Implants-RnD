using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SignalPreset")]
public class SignalPreset : ScriptableObject {

    [Header("Volume / Frequency")]
    [Range(0.0f, 1.0f)]
    public float masterVolume = 0.5f;
    [Range(0, 300)]
    public double mainFrequency = 500;
    [Space(10)]

    [Header("Tone Adjustment")]
    public bool useSinusAudioWave;
    [Range(0.0f, 1.0f)]
    public float sinusAudioWaveIntensity = 0.25f;
    [Space(5)]
    public bool useSquareAudioWave;
    [Range(0.0f, 1.0f)]
    public float squareAudioWaveIntensity = 0.25f;
    [Space(5)]
    public bool useSawAudioWave;
    [Range(0.0f, 1.0f)]
    public float sawAudioWaveIntensity = 0.25f;
    [Space(5)]
    public bool useDCAudio;
    [Range(0.0f, 1.0f)]
    public float dcAudioIntensity = 0.25f;

    [Space(10)]

    [Header("Amplitude Modulation")]
    public bool useAmplitudeModulation;
    [Range(0.0f, 30.0f)]
    public float amplitudeModulationOscillatorFrequency = 1.0f;
    [Header("Frequency Modulation")]
    public bool useFrequencyModulation;
    [Range(0.0f, 30.0f)]
    public float frequencyModulationOscillatorFrequency = 1.0f;
    [Range(1.0f, 100.0f)]
    public float frequencyModulationOscillatorIntensity = 10.0f;

    [Header("Out Values")]
    [Range(0.0f, 1.0f)]
    public float amplitudeModulationRangeOut;
    [Range(0.0f, 1.0f)]
    public float frequencyModulationRangeOut;
}
