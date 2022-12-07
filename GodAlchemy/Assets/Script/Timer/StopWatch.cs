using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StopWatch : MonoBehaviour
{
    private bool stopWatchActive;
    public float currentTime;
    public float endTime;
    // Start is called before the first frame update
    void Start()
    {
        stopWatchActive = false;
        currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(stopWatchActive)
        {
            if (currentTime < endTime)
                currentTime += Time.deltaTime;
            else
                stopWatchActive = false;

        }
        
    }

    public void SetCurrentTime(float time)
    {
        currentTime = time;
    }

    public void SetEndTime(float time)
    {
        endTime = time;
    }

    public int GetCurrentTimeInSecond()
    {
        return TimeSpan.FromSeconds(currentTime).Seconds;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public void StopCount()
    {
        stopWatchActive = false;
        currentTime = 0;
    }

    public void StartCount()
    {
        stopWatchActive = true;
        currentTime = 0;
    }

    public bool IsFinished()
    {
        return currentTime >= endTime;
    }

    public bool IsActive()
    {
        return stopWatchActive;
    }
}
