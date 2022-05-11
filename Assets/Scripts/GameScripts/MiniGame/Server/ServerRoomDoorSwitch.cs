using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerRoomDoorSwitch : MonoBehaviour
{

    /*
     Handles the logic for the switches in the server task.
     Contributors : Seth
     */
    public GameObject doorSwitchManager;
    private int currentValue = 0;
    public int switchID;

    //Send to network
    public void OnLeverChange(GameObject player) 
    {
        player.GetComponent<PlayerNetworkManager>().OnLeverUp(gameObject);
    }

    //Recieved for all players via network manager 
    public void UpdateOnLeverChange()
    {
        if (currentValue == 1)
        {
            currentValue = 0;
            transform.GetChild(0).GetComponent<Animator>().Play("Off");
        }
        else
        {
            currentValue = 1;
            transform.GetChild(0).GetComponent<Animator>().Play("On");
        }
        SendCurrentState();
    }

    public void SendCurrentState() 
    {
        doorSwitchManager.GetComponent<DoorSwitchManagers>().RecieveNewSwitchState(switchID, currentValue);
    }
}
