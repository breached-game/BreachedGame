using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class InteractiveMap : MonoBehaviour
{
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
        roomText = RoomInfo.GetComponent<TextMeshPro>();
    }

    public void setRoomInfo(string roomInfo, GameObject room)
    {
        if (localPlayerInside != true) return;
        if (activeRoom != null)
        {
            if (activeRoom != room) activeRoom.GetComponent<MeshRenderer>().material = blueHalogram;
        }
        activeRoom = room;
        room.GetComponent<MeshRenderer>().material = greenHalogram;
        roomText.text = roomInfo;
    }
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
