 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public GameObject Alarms;
    public GameObject OverheadLights;
    public GameObject FloorLights;

    public Material FloorLightsOn;
    public Material FloorLightsOff;

    public void TurnPressureAlarmOff()
    {
        for(int i = 0; i < Alarms.transform.childCount; i++)
        {
            Alarms.transform.GetChild(i).GetComponent<LightFlickerEffect>().alarmOn = false;
            Alarms.transform.GetChild(i).GetComponent<AudioSource>().Stop();
        }
        for (int i = 0; i < OverheadLights.transform.childCount; i++)
        {
            OverheadLights.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(true);
            OverheadLights.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(true);
            OverheadLights.transform.GetChild(i).transform.GetChild(3).gameObject.SetActive(true);
        }
        for (int i = 0; i < FloorLights.transform.childCount; i++)
        {
            FloorLights.transform.GetChild(i).transform.GetComponent<MeshRenderer>().material = FloorLightsOff;
        }
    }
    public void TurnPressureAlarmOn()
    {
        for (int i = 0; i < Alarms.transform.childCount; i++)
        {
            Alarms.transform.GetChild(i).GetComponent<LightFlickerEffect>().startAlarms();
            Alarms.transform.GetChild(i).GetComponent<AudioSource>().Play();
        }
        for (int i = 0; i < OverheadLights.transform.childCount; i++)
        {
            OverheadLights.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
            OverheadLights.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(false);
            OverheadLights.transform.GetChild(i).transform.GetChild(3).gameObject.SetActive(true);
        }
        for (int i = 0; i < FloorLights.transform.childCount; i++)
        {
            FloorLights.transform.GetChild(i).transform.GetComponent<MeshRenderer>().material = FloorLightsOn;
        }
    }
}
