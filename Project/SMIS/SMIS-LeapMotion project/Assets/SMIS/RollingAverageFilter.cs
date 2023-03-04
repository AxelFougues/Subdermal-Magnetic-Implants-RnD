using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingAverageFilter{ //SHOULD BE MADE GENERIC
    
    protected int bufferDimension;
    protected List<Queue<Vector3>> vectorBuffer;
    protected List<Queue<float>> floatBuffer;

    public RollingAverageFilter(int bufferDimension, int channels) {
        this.bufferDimension = bufferDimension;
        vectorBuffer = new List<Queue<Vector3>>();
        floatBuffer = new List<Queue<float>>();
        for (int i = 0; i < channels; i++) vectorBuffer.Add(new Queue<Vector3>()); //MEH
        for (int i = 0; i < channels; i++) floatBuffer.Add(new Queue<float>()); //MEH
    }

    public RollingAverageFilter(int bufferDimension) {
        this.bufferDimension = bufferDimension;
        vectorBuffer = new List<Queue<Vector3>>();
        floatBuffer = new List<Queue<float>>();
        vectorBuffer.Add(new Queue<Vector3>());
        floatBuffer.Add(new Queue<float>());
    }

    public Vector3 getValue(Vector3 unfiltered, int channel) {
        if (bufferDimension < 2) return unfiltered;
        vectorBuffer[channel].Enqueue(unfiltered);
        if (vectorBuffer[channel].Count > bufferDimension) vectorBuffer[channel].Dequeue();
        Vector3 filtered = Vector3.zero;
        foreach (Vector3 value in vectorBuffer[channel]) filtered += value;
        return filtered / vectorBuffer[channel].Count;
    }

    public Vector3 getValue(Vector3 unfiltered) {
        return getValue(unfiltered, 0);
    }

    public float getValue(float unfiltered, int channel) {
        if (bufferDimension < 2) return unfiltered;
        floatBuffer[channel].Enqueue(unfiltered);
        if (floatBuffer[channel].Count > bufferDimension) floatBuffer[channel].Dequeue();
        float filtered = 0;
        foreach (float value in floatBuffer[channel]) filtered += value;
        return filtered / floatBuffer[channel].Count;
    }

    public float getValue(float unfiltered) {
        return getValue(unfiltered, 0);
    }

    public void clear(int channel = 0) { vectorBuffer[channel].Clear(); floatBuffer[channel].Clear(); }

}
