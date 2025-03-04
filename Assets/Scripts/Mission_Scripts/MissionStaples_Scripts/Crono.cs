using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crono : MonoBehaviour
{
    private float elapsedTime;
    private bool isRunning = false;

    void Update()
    {
        if (isRunning)
        {
            // Add the time elapsed since the last frame
            elapsedTime += Time.deltaTime;

        }
    }

    // Method to start the stopwatch
    public void StartStopwatch()
    {
        isRunning = true;
        elapsedTime = 0f;  // Reset time if needed
    }

    public void PlayStopwatch()
    {
        isRunning = true;
    }
    // Method to stop the stopwatch
    public void StopStopwatch()
    {
        isRunning = false;
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }


}