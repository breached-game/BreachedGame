using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public GameObject Alarms;
    public GameObject OverheadLights;

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
        }
    }
    public void TurnPressureAlarmOn()
    {
        for (int i = 0; i < Alarms.transform.childCount; i++)
        {
            Alarms.transform.GetChild(i).GetComponent<LightFlickerEffect>().alarmOn = true;
            Alarms.transform.GetChild(i).GetComponent<AudioSource>().Play();
        }
        for (int i = 0; i < OverheadLights.transform.childCount; i++)
        {
            OverheadLights.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
            OverheadLights.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(false);
        }
    }
}
