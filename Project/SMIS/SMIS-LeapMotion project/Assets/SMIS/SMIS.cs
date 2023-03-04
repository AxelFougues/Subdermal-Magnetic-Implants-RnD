using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BTSensorBridge))]
[ExecuteInEditMode]
public class SMIS : MonoBehaviour {

    [Header("Links")]
    public SignalGenerator left;
    public SignalGenerator right;
    public AudioSource leftSource;
    public AudioSource rightSource;
    public SignalPreset defaultSignal;
    public SignalPreset tactileFeedbackSignal;
    public SignalPreset sensorFeedbackSignal;
    public SMISSettings smisSettings;

    [Space]
    [Header("Tactile Settings")]
    [SerializeField]
    public List<Collider> virtualFingers;
    public bool simulateMotion = true;
    [Tooltip("Velocity in mm/s")]
    public float minSlidingVelocity = 1f;

    [Space]
    [Header("UI Texts")]
    public Text pressure;
    public Text surfaceVelocity;
    public Text feedbackFrequency;

    [Space]
    [Header("Test Sequence Settings")]
    [Range(0, 1)]
    public int channel;
    [Range(0, 10)]
    public float testDuration;
    public bool pauses;
    [Range(0, 300)]
    public float minFrequency;
    [Range(0, 300)]
    public float maxFrequency;
    [Range(0.1f, 50f)]
    public float step;
    public bool equalized;
    public bool sine;
    public bool square;
    public bool saw;

    [HideInInspector]
    public List<SignalGenerator> channels;
    [HideInInspector]
    public List<AudioSource> sources;

    [HideInInspector]
    public IEnumerator[] channelFeedbacks;


    RollingAverageFilter modulationFilter = new RollingAverageFilter(50, 2);

    private void OnEnable() {

        left.channel = 0;
        right.channel = 1;

        channels = new List<SignalGenerator>();
        channels.Add(left);
        channels.Add(right);
        sources = new List<AudioSource>();
        sources.Add(leftSource);
        sources.Add(rightSource);

        channelFeedbacks = new IEnumerator[2];

        disableFeedback(0);
        disableFeedback(1);

    }



    public void disableFeedback(int channel) {
        if (channel < channels.Count) {
            channels[channel].enabled = false;
            if (channelFeedbacks[channel] != null) {
                StopCoroutine(channelFeedbacks[channel]);
                channelFeedbacks[channel] = null;
            }
        }
    }

    public void enableFeedback(int channel) {
        disableFeedback(channel); //reset if currently used
        channels[channel].loadPreset(defaultSignal);
        channels[channel].enabled = true;
    }


    //AUTOMATED SENSITIVITY TESTING
    public void startTestSequence() {
        if (sine) startTestSequenceSine(channel, testDuration, pauses, minFrequency, maxFrequency, step, equalized);
        else if (square) startTestSequenceSquare(channel, testDuration, pauses, minFrequency, maxFrequency, step, equalized);
        else if (saw) startTestSequenceSaw(channel, testDuration, pauses, minFrequency, maxFrequency, step, equalized);
    }
    public void startTestSequenceSine(int channel, float testDuration, bool pauses, float minFrequency, float maxFrequency, float step, bool equalized) {
        enableFeedback(channel);
        channels[channel].useSinusAudioWave = true;
        channels[channel].sinusAudioWaveIntensity = 1;
        startFeedbackCoroutine(channel, testSequence(channel, testDuration, pauses, minFrequency, maxFrequency, step, equalized));
    }
    public void startTestSequenceSquare(int channel, float testDuration, bool pauses, float minFrequency, float maxFrequency, float step, bool equalized) {
        enableFeedback(channel);
        channels[channel].useSquareAudioWave = true;
        channels[channel].squareAudioWaveIntensity = 1;
        startFeedbackCoroutine(channel, testSequence(channel, testDuration, pauses, minFrequency, maxFrequency, step, equalized));
    }
    public void startTestSequenceSaw(int channel, float testDuration, bool pauses, float minFrequency, float maxFrequency, float step, bool equalized) {
        enableFeedback(channel);
        channels[channel].useSawAudioWave = true;
        channels[channel].sawAudioWaveIntensity = 1;
        startFeedbackCoroutine(channel, testSequence(channel, testDuration, pauses, minFrequency, maxFrequency, step, equalized));
    }
    IEnumerator testSequence(int channel, float testDuration, bool pauses, float minFrequency, float maxFrequency, float step, bool equalized) {
        channels[channel].mainFrequency = minFrequency;
        int i = 0;
        do {
            if (i++ % 2 == 0 || !pauses) {
                if (equalized) {
                    if (channels[channel].useSinusAudioWave) channels[channel].masterVolume = SensitivityEq.equalizeSineAmplitude(1, (float)channels[channel].mainFrequency);
                    else if (channels[channel].useSquareAudioWave) channels[channel].masterVolume = SensitivityEq.equalizeSquareAmplitude(1, (float)channels[channel].mainFrequency);
                    else if (channels[channel].useSawAudioWave) channels[channel].masterVolume = SensitivityEq.equalizeSawAmplitude(1, (float)channels[channel].mainFrequency);
                } else channels[channel].masterVolume = 1;
                Debug.Log("Testing at freq: " + channels[channel].mainFrequency);
                channels[channel].mainFrequency += Mathf.Sign(maxFrequency - minFrequency) * step;
            } else {
                channels[channel].masterVolume = 0;
                Debug.Log("Test pause");
            }
            yield return new WaitForSeconds(testDuration);
        } while (isBetween(channels[channel].mainFrequency, minFrequency, maxFrequency));
        disableFeedback(channel);
    }

    //XR TACTILE FEEDBACK
    public void startTactileFeedback(int channel, TouchableObject objectTouched) {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        if (channel < channels.Count && channelFeedbacks[channel] == null) {
            enableFeedback(channel);
            channels[channel].loadPreset(tactileFeedbackSignal);
            startFeedbackCoroutine(channel, TactileFeedback(channel, objectTouched));
        }
#endif
    }
    
    IEnumerator TactileFeedback(int channel, TouchableObject objectTouched) {
        channels[channel].masterVolume = 1;
        channels[channel].useAmplitudeModulation = true;
        while (true && objectTouched != null) {
            if (objectTouched.beingTouched) {
                channels[channel].mainFrequency = objectTouched.materialProperties.getStaticFrequency();
                channels[channel].masterVolume = objectTouched.materialProperties.getPressureAmplitude(objectTouched.contactParameters[channel].pDistance);
                //MODULATION
                if (simulateMotion && objectTouched.contactParameters[channel].pPerpendicularVelocity > minSlidingVelocity) {
                    channels[channel].amplitudeModulationOscillatorFrequency = modulationFilter.getValue(objectTouched.materialProperties.getTextureFrequency(objectTouched.contactParameters[channel].pPerpendicularVelocity), channel);
                } else {
                    channels[channel].amplitudeModulationOscillatorFrequency = 0;
                }
            }
            if (pressure != null) pressure.text = "Pressure: " + channels[channel].masterVolume * 100f + " %";
            if (surfaceVelocity != null) surfaceVelocity.text = "Surface velocity: " + objectTouched.contactParameters[channel].pPerpendicularVelocity + " mm/s";
            if (feedbackFrequency != null) feedbackFrequency.text = "Mod frequency: " + channels[channel].amplitudeModulationOscillatorFrequency + " Hz";
            yield return new WaitForFixedUpdate();
        }
    }
    public void stopTactileFeedback(int channel, TouchableObject objectTouched) {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        if (channel < channels.Count) {
            disableFeedback(channel);
        }
#endif
    }


    //AUDIO FEEDBACK
    public void doAudioFeedback(int channel, AudioClip clip) {
        if (channel < channels.Count && clip != null) {
            disableFeedback(channel); //removing filter
            sources[channel].PlayOneShot(clip);
            startFeedbackCoroutine(channel, AudioFeedback(channel, clip));
        }
    }

    IEnumerator AudioFeedback(int channel, AudioClip clip) {
        yield return new WaitForSeconds(clip.length);
        disableFeedback(channel); //free up the channel
    }

    

    //DIRECT FEEDBACK
    public void doDirectFeedback(int channel, float intensity) { doDirectFeedback(channel, intensity, AnimationCurve.Linear(0, 0, 1, 1)); }
    public void doDirectFeedback(int channel, float intensity, float minFreq, float maxFreq) { doDirectFeedback(channel, intensity, AnimationCurve.Linear(0, 0, 1, 1), minFreq, maxFreq); }
    public void doDirectFeedback(int channel, float intensity, AnimationCurve curve, float minFreq = 30, float maxFreq = 100) {
        if (channel < channels.Count) {
            enableFeedback(channel);
            channels[channel].loadPreset(defaultSignal);
            intensity = curve.Evaluate(intensity);
            channels[channel].mainFrequency = mapValue(intensity, 0, 1, minFreq, maxFreq);
        }
    }

    //####################################################################################### UTILITIES



    bool isBetween(double testValue, double bound1, double bound2) {
        if (bound1 > bound2)
            return testValue >= bound2 && testValue <= bound1;
        return testValue >= bound1 && testValue <= bound2;
    }

    float mapValue(float referenceValue, float fromMin, float fromMax, float toMin, float toMax) {
        /* This function maps (converts) a Float value from one range to another */
        return toMin + (referenceValue - fromMin) * (toMax - toMin) / (fromMax - fromMin);
    }

    void startFeedbackCoroutine(int channel, IEnumerator coroutine) {
        channelFeedbacks[channel] = coroutine;
        StartCoroutine(coroutine);
    }

}
