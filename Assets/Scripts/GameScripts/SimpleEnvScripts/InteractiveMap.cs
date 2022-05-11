using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class InteractiveMap : MonoBehaviour
{
    /*
        SCRIPT FOR MANAGING THE INTERACTIVE MAP IN THE CONTROL ROOM

        Contributors: Andrew Morgan
    */
    public bool topFloor = true;
    public GameObject RoomInfo;
    public GameObject Map;
    public Material blueHalogram;
    public Material greenHalogram;
    private TextMeshPro roomText;
    private GameObject activeRoom;
    private bool localPlayerInside = false;

    // Start is called before the first frame update
    void Start()
    {
        // Sets variable to Unity component for later reference
        roomText = RoomInfo.GetComponent<TextMeshPro>();
    }

    public void setRoomInfo(string roomInfo, GameObject room)
    {
        // Checks whether player is close enough to interact
        if (localPlayerInside != true) return;
        // Sets current highlighted room to be a standard blue hologram
        if (activeRoom != null)
        {
            if (activeRoom != room) activeRoom.GetComponent<MeshRenderer>().material = blueHalogram;
        }
        // Sets currently selected room to be green
        activeRoom = room;
        room.GetComponent<MeshRenderer>().material = greenHalogram;
        roomText.text = roomInfo;
    }

    // Used to changed the floor that the map is displaying
    public void setFloor(int floor)
    {
        if (localPlayerInside != true) return;
        if (floor == 1)
        {
            topFloor = false;
            Map.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            topFloor = true;
            Map.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    // Checks whether collided player is local and sets that they are close enough to interact
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.transform.tag == "Player")
        {
            if (other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                localPlayerInside = true;
            }
        }
    }

    // Checks whether collided player is local and sets that they are now too far away
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.tag == "Player")
        {
            if (other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                localPlayerInside = false;
            }
        }
    }
}
