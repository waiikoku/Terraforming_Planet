using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sun;
    public float secondsInFullDay = 120f;
    public int minutesInFullDay;
    public float hoursInFullDay;
    [Range(0, 1)]
    public float currentTimeOfDay = 0;
    [HideInInspector]
    public float timeMultiplier = 1f;

    float sunInitialIntensity;
    public static bool Day;
    public static bool Night;

    void Start()
    {
        sunInitialIntensity = sun.intensity;
        if (minutesInFullDay != (int)(secondsInFullDay / 60))
        {
            minutesInFullDay = (int)(secondsInFullDay / 60);
        }
        if (hoursInFullDay != (minutesInFullDay / 60))
        {
            hoursInFullDay = (minutesInFullDay / 60);
        }
    }

    void Update()
    {
        UpdateSun();

        currentTimeOfDay += (Time.deltaTime / secondsInFullDay) * timeMultiplier;

        if (currentTimeOfDay >= 1)
        {
            currentTimeOfDay = 0;
        }
        if(currentTimeOfDay >= 0.26f && currentTimeOfDay <= 0.74f)
        {
            if (Night)
            {
                Night = false;
            }
            if (!Day)
            {
                Day = true;
            }
        }
        else
        {
            if (Day)
            {
                Day = false;
            }
            if (!Night)
            {
                Night = true;
            }
        }
    }

    void UpdateSun()
    {
        sun.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 170, 0);

        float intensityMultiplier = 1;
        if (currentTimeOfDay <= 0.23f || currentTimeOfDay >= 0.75f)
        {
            intensityMultiplier = 0;
        }
        else if (currentTimeOfDay <= 0.25f)
        {
            intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - 0.23f) * (1 / 0.02f));
        }
        else if (currentTimeOfDay >= 0.73f)
        {
            intensityMultiplier = Mathf.Clamp01(1 - ((currentTimeOfDay - 0.73f) * (1 / 0.02f)));
        }

        sun.intensity = sunInitialIntensity * intensityMultiplier;
    }
}
