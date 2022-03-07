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
        if (DISABLE_AFTER_TESTING)
        {
            lights.GetComponent<LightManager>().TurnPressureAlarmOn();
            DISABLE_AFTER_TESTING = true;
        }
        else
        {
            lights.GetComponent<LightManager>().TurnPressureAlarmOff();
            DISABLE_AFTER_TESTING = false;
        } 
    }
