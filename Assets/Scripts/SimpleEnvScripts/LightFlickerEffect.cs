using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// Written by Steve Streeting 2017
// License: CC0 Public Domain http://creativecommons.org/publicdomain/zero/1.0/

/// <summary>
/// Component which will flicker a linked light while active by changing its
/// intensity between the min and max values given. The flickering can be
/// sharp or smoothed depending on the value of the smoothing parameter.
///
/// Just activate / deactivate this component as usual to pause / resume flicker
/// </summary>
public class LightFlickerEffect : MonoBehaviour
{
    [Tooltip("External light to flicker; you can leave this null if you attach script to a light")]
    public new Light light;
    public float intensity = 3f;
    public float interval = 0.5f;

    public GameObject actualLight;
    public Material onLight;
    public Material offLight;
    public bool alarmOn = true;

    void Start()
    {
        //Start the coroutine we define below named ExampleCoroutine.
        StartCoroutine(LightFlicker());
    }
    public void startAlarms()
    {
        StartCoroutine(LightFlicker());
    }

    IEnumerator LightFlicker()
    {
        while (alarmOn)
        {
            light.intensity = intensity;
            actualLight.GetComponent<Renderer>().material = onLight;
            yield return new WaitForSeconds(interval);
            actualLight.GetComponent<Renderer>().material = offLight;
            light.intensity = 0;
            yield return new WaitForSeconds(interval);
        }
    }

}
