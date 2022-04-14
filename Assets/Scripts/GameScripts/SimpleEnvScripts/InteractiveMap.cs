using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractiveMap : MonoBehaviour
{
    public bool topFloor = true;
    public GameObject RoomInfo;
    public GameObject Map;
    public Material blueHalogram;
    public Material greenHalogram;
    private TextMeshPro roomText;
    private GameObject activeRoom;
    // Start is called before the first frame update
    void Start()
    {
        roomText = RoomInfo.GetComponent<TextMeshPro>();
    }

    public void setRoomInfo(string info, GameObject room)
    {
        if (activeRoom != null)
        {
            if (activeRoom != room) activeRoom.GetComponent<MeshRenderer>().material = blueHalogram;
        }
        activeRoom = room;
        room.GetComponent<MeshRenderer>().material = greenHalogram;
        roomText.text = info;
    }
    public void setFloor(int floor)
    {
        if(floor == 1)
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
}
