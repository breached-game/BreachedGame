 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    /*
        SCRIPT FOR MANAGING THE LIGHTS AROUND THE SUBMARINE.
        THIS INCLUDES THE ALARM LIGHTS AND OVERHEAD/FLOOR LIGHTS

        Contributors: Andrew Morgan
    */
    public GameObject Alarms;
    public GameObject OverheadLights;
    public GameObject FloorLights;

    public Material FloorLightsOn;
    public Material FloorLightsOff;

    // Called when the alarm lights need to stop flashing
    public void TurnPressureAlarmOff()
    {
        // Iterates through all alarm lights
        for(int i = 0; i < Alarms.transform.childCount; i++)
        {
            // Turns off alarm lights flashing
            Alarms.transform.GetChild(i).GetComponent<LightFlickerEffect>().alarmOn = false;
            // Also turns off alarm sound
            Alarms.transform.GetChild(i).GetComponent<AudioSource>().Stop();
        }
        for (int i = 0; i < OverheadLights.transform.childCount; i++)
        {
            // Turns on actual overhead lights
            OverheadLights.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(true);
            OverheadLights.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(false);
            OverheadLights.transform.GetChild(i).transform.GetChild(3).gameObject.SetActive(true);
        }
        for (int i = 0; i < FloorLights.transform.childCount; i++)
        {
            // Floor lights are turned off unless alarms are on
            FloorLights.transform.GetChild(i).transform.GetComponent<MeshRenderer>().material = FloorLightsOff;
        }
    }
    public void TurnPressureAlarmOn()
    {
        // Iterates through all alarm lights
        for (int i = 0; i < Alarms.transform.childCount; i++)
        {
            // Start alarms flashing
            Alarms.transform.GetChild(i).GetComponent<LightFlickerEffect>().startAlarms();
            // Also plays alarm sound
            Alarms.transform.GetChild(i).GetComponent<AudioSource>().Play();
        }
        for (int i = 0; i < OverheadLights.transform.childCount; i++)
        {
            // Turns off actual overhead lights but leaves orb there so it doesn't look
            // like the light just disappears
            OverheadLights.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(false);
            OverheadLights.transform.GetChild(i).transform.GetChild(3).gameObject.SetActive(false);
        }
        for (int i = 0; i < FloorLights.transform.childCount; i++)
        {
            FloorLights.transform.GetChild(i).transform.GetComponent<MeshRenderer>().material = FloorLightsOn;
        }
    }
}
