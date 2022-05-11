using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PressureAlarm : NetworkBehaviour
{
    /*
        SIMPLE SCRIPT USED A MIDDLE MAN FOR TURNING THE ALARMS ON AND OFF
        FROM A SCRIPT WITH A NETWORK IDENTITY. AVOIDS ALL ALARMS HAVING 
        TO BE NETWORKED

        Contributors: Andrew Morgan
    */
    public GameObject lights;

    public void StartAlarm()
    {
        lights.GetComponent<LightManager>().TurnPressureAlarmOn();
    }

    public void StopAlarm()
    {
        lights.GetComponent<LightManager>().TurnPressureAlarmOff();
    }
}
