using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChangeLevelButton : MonoBehaviour
{
    public GameObject mapManager;
    public int floor = 1;
    private void OnMouseDown()
    {
        mapManager.GetComponent<InteractiveMap>().setFloor(floor);
    }
}
