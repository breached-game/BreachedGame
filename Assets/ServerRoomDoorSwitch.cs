using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerRoomDoorSwitch : MonoBehaviour
{
    public GameObject doorSwitchManager;
    private int currentValue = 0;
    public int switchID;

    public void OnLeverUp() 
    {
        currentValue = 0;
        SendCurrentState();
    }
    public void OnLeverDown() 
    {
        currentValue = 1;
        SendCurrentState();
    }

    public void SendCurrentState() 
    {
        doorSwitchManager.GetComponent<DoorSwitchManagers>().RecieveNewSwitchState(switchID, currentValue);
    }



}
