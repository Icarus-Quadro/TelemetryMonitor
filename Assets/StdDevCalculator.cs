using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StdDevCalculator
{ 
    private readonly int mStepCount;
    private readonly Vector3[] mSamples;
    private int mStep = 0;
    
    public StdDevCalculator(int stepCount)
    {
        mStepCount = stepCount;
        mSamples = new Vector3[mStepCount];
    }

    public float AddSample(Vector3 sample)
    {
        mSamples[mStep] = sample;
        mStep = (mStep + 1) % mStepCount;

        Vector3 mean = new Vector3();

        for (int i = 0; i < mStepCount; ++i)
        {
            mean += mSamples[i];
        }

        mean /= mStepCount;

        float stdDev = 0;

        for (int i = 0; i < mStepCount; ++i)
        {
            var diff = mean - mSamples[i];
            stdDev += Vector3.Dot(diff, diff);
        }

        return Mathf.Sqrt(stdDev / mStepCount);
    }
}
