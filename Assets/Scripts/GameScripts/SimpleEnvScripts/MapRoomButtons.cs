using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRoomButtons : MonoBehaviour
{
    public GameObject mapManager;
    public string roomInfo;
    public bool topFloor = false;
    private void OnMouseOver()
    {
        if(topFloor == mapManager.GetComponent<InteractiveMap>().topFloor)
        {
            mapManager.GetComponent<InteractiveMap>().setRoomInfo(roomInfo, this.gameObject);
        }
    }
}
