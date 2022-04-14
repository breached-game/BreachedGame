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
    private PlayerNetworkManager playerNetworkManager;

    // Start is called before the first frame update
    void Start()
    {
        roomText = RoomInfo.GetComponent<TextMeshPro>();
    }

    public void setRoomInfo(string info, GameObject room)
    {
        if (playerNetworkManager == null) return;
        playerNetworkManager.OnSetRoom(info, this.gameObject, room);
    }
    public void setFloor(int floor)
    {
        if (playerNetworkManager == null) return;
        playerNetworkManager.OnSetFloor(floor, this.gameObject);
    }
    public void callSetFloor(int floor)
    {
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
    public void callSetRoom(string roomInfo, GameObject room)
    {
        if (activeRoom != null)
        {
            if (activeRoom != room) activeRoom.GetComponent<MeshRenderer>().material = blueHalogram;
        }
        activeRoom = room;
        room.GetComponent<MeshRenderer>().material = greenHalogram;
        roomText.text = roomInfo;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.transform.tag == "Player")
        {
            if (other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                playerNetworkManager = other.gameObject.GetComponent<PlayerNetworkManager>();
            }
        }
    }
}
