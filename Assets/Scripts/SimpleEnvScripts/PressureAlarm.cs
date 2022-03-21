using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PressureAlarm : NetworkBehaviour
{
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
