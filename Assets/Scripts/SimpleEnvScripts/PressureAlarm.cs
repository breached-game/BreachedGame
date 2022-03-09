using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PressureAlarm : NetworkBehaviour
{
    public GameObject lights;
    private bool DISABLE_AFTER_TESTING = false;
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
        yield return new WaitForSeconds(10);
        lights.GetComponent<LightManager>().TurnPressureAlarmOn();
    }
}
