using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Texture2D skyboxNight;
    [SerializeField] private Texture2D skyboxSunrise;
    [SerializeField] private Texture2D skyboxDay;
    [SerializeField] private Texture2D skyboxSunset;

    [SerializeField] private Gradient graddientNightToSunrise;
    [SerializeField] private Gradient graddientSunriseToDay;
    [SerializeField] private Gradient graddientDayToSunset;
    [SerializeField] private Gradient graddientSunsetToNight;

    [SerializeField] private float nightLightIntensity = 0.2f;
    [SerializeField] private float dayLightIntensity = 1f;
    [SerializeField] private float dayLightMultiplierIntensity = 1f;
    [SerializeField] private float intensityChangeSpeed = 10;
    [SerializeField] private Light globalLight;
    [SerializeField] float speed;

    [SerializeField] private int minutes;

    public int Minutes
    { get { return minutes; } set { minutes = value; OnMinutesChange(value); } }

    [SerializeField] private int hours = 6;

    public int Hours
    { get { return hours; } set { hours = value; OnHoursChange(value); } }

    [SerializeField] private int days;

    public int Days
    { get { return days; } set { days = value; } }

    [SerializeField] private float tempSecond;


    public void InitDefault()
    {
        // set based on starting hours
        SetSkybox(skyboxDay, skyboxSunset);
        SetLight(graddientDayToSunset);
    }

    public void Update()
    {
        tempSecond += Time.deltaTime * speed;

        if (tempSecond >= 1)
        {
            Minutes += 1;
            tempSecond = 0;
        }
    }

    private void OnMinutesChange(int value)
    {
        StartCoroutine(LerpLightAngle(1 / speed));
        if (value >= 60)
        {
            Hours++;
            minutes = 0;
        }
        if (Hours >= 24)
        {
            Hours = 0;
            Days++;
        }
    }

    private void OnHoursChange(int value)
    {
        if (value == 6)
        {
            StartCoroutine(LerpLightIntesity(1 / speed * intensityChangeSpeed, dayLightIntensity));
            StartCoroutine(LerpSkybox(skyboxNight, skyboxSunrise, 10f));
            StartCoroutine(LerpLight(graddientNightToSunrise, 10f));
            StartCoroutine(LerpIntesityMultiplier(intensityChangeSpeed, dayLightMultiplierIntensity));
        }
        else if (value == 8)
        {
            StartCoroutine(LerpSkybox(skyboxSunrise, skyboxDay, 10f));
            StartCoroutine(LerpLight(graddientSunriseToDay, 10f));
        }
        else if (value == 18)
        {
            StartCoroutine(LerpSkybox(skyboxDay, skyboxSunset, 10f));
            StartCoroutine(LerpLight(graddientDayToSunset, 10f));
        }
        else if (value == 22)
        {
            StartCoroutine(LerpLightIntesity(1 / speed * intensityChangeSpeed, nightLightIntensity));
            StartCoroutine(LerpSkybox(skyboxSunset, skyboxNight, 10f));
            StartCoroutine(LerpLight(graddientSunsetToNight, 10f));
            StartCoroutine(LerpIntesityMultiplier(intensityChangeSpeed, nightLightIntensity));
        }
    }

    // will be called on run to set the skybox to morning
    private void SetSkybox(Texture2D a, Texture2D b)
    {
        RenderSettings.skybox.SetTexture("_Texture1", a);
        RenderSettings.skybox.SetTexture("_Texture2", b);
        RenderSettings.skybox.SetFloat("_Blend", 0);
    }

    private IEnumerator LerpSkybox(Texture2D a, Texture2D b, float time)
    {
        RenderSettings.skybox.SetTexture("_Texture1", a);
        RenderSettings.skybox.SetTexture("_Texture2", b);
        RenderSettings.skybox.SetFloat("_Blend", 0);
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            RenderSettings.skybox.SetFloat("_Blend", i / time);
            yield return null;
        }
        RenderSettings.skybox.SetTexture("_Texture1", b);
    }

    private void SetLight(Gradient lightGradient)
    {
        globalLight.color = lightGradient.Evaluate(0);
        RenderSettings.fogColor = globalLight.color;
    }

    private IEnumerator LerpLight(Gradient lightGradient, float time)
    {
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            globalLight.color = lightGradient.Evaluate(i / time);
            RenderSettings.fogColor = globalLight.color;
            yield return null;
        }
    }

    private IEnumerator LerpLightAngle(float duration)
    {
        float timeElapsed = 0;

        Quaternion startRotation = globalLight.transform.rotation;
        Quaternion nextAngle = Quaternion.Euler(0, ((1f / (1440f / 4f)) * 360f), 0) * startRotation;
        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;

            globalLight.transform.rotation = Quaternion.Slerp(startRotation, nextAngle, t);

            timeElapsed += Time.deltaTime;

            yield return null;
        }
        globalLight.transform.rotation = nextAngle;
    }

    private IEnumerator LerpLightIntesity(float duration, float targetIntensity)
    {
        float timeElapsed = 0;

        float startIntensity = globalLight.intensity;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;

            globalLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, t);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        globalLight.intensity = targetIntensity;
    }

    private IEnumerator LerpIntesityMultiplier(float duration, float targetIntensity)
    {
        float timeElapsed = 0;

        float startIntensity = globalLight.intensity;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;

            RenderSettings.ambientIntensity = Mathf.Lerp(startIntensity, targetIntensity, t);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        RenderSettings.ambientIntensity = targetIntensity;
    }

    public void SetTime(int _minutes, int _hours, int _days)
    {
        minutes = _minutes;
        hours = _hours;
        days = _days;

        if (hours < 6 || hours >= 22)
        {
            StartCoroutine(LerpLightIntesity(1 / speed, nightLightIntensity));
            SetSkybox(skyboxNight, skyboxSunrise);
            SetLight(graddientNightToSunrise);
            StartCoroutine(LerpIntesityMultiplier(1/speed, nightLightIntensity));
        }
        else if (hours >= 6 && hours < 8)
        {
            StartCoroutine(LerpLightIntesity(1 / speed, dayLightIntensity));
            SetSkybox(skyboxSunrise, skyboxDay);
            SetLight(graddientSunriseToDay);
            StartCoroutine(LerpIntesityMultiplier(1 / speed, dayLightMultiplierIntensity));
        }
        else if (hours >= 8 && hours < 18)
        {
            StartCoroutine(LerpLightIntesity(1 / speed, dayLightIntensity));
            SetSkybox(skyboxDay, skyboxSunset);
            SetLight(graddientDayToSunset);
            StartCoroutine(LerpIntesityMultiplier(1 / speed, dayLightMultiplierIntensity));
        }
        else if (hours >= 18 && hours < 22)
        {
            StartCoroutine(LerpLightIntesity(1 / speed, dayLightIntensity));
            SetSkybox(skyboxSunset, skyboxNight);
            SetLight(graddientSunsetToNight);
            StartCoroutine(LerpIntesityMultiplier(1 / speed, dayLightMultiplierIntensity));
        }
    }
}
