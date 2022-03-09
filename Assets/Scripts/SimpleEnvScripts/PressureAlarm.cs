using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PressureAlarm : NetworkBehaviour
{
    public GameObject lights;
    [Command]
    public void CmdPressureAlarmPress()
    {
        PressureAlarmPress();
    }
    [ClientRpc]
    void PressureAlarmPress()
    {
        lights.GetComponent<LightManager>().TurnPressureAlarmOff();
        StartCoroutine(AlarmTimer());
    }

    IEnumerator AlarmTimer()
    {
        yield return new WaitForSeconds(30);
        lights.GetComponent<LightManager>().TurnPressureAlarmOn();
    }
}
