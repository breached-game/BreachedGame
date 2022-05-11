using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRoomButtons : MonoBehaviour
{
    /*
        SCRIPT FOR DISPLAYING THE NAMES OF ROOMS ON THE INTERACTIVE MAP

        Contributors: Andrew Morgan
    */
    public GameObject mapManager;
    public string roomInfo;
    public bool topFloor = false;

    // Detects when a player's crosshair is over a room
    private void OnMouseOver()
    {
        if(topFloor == mapManager.GetComponent<InteractiveMap>().topFloor)
        {
            // mapManager will check whether the player is close enough to interact
            mapManager.GetComponent<InteractiveMap>().setRoomInfo(roomInfo, this.gameObject);
        }
    }
}
