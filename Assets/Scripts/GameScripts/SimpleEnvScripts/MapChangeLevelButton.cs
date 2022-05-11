using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChangeLevelButton : MonoBehaviour
{
    /*
        HANDLES THE BUTTON ON THE INTERACTIVE MAP FOR CHANGING FLOORS

        Contributors: Andrew Morgan
    */
    public GameObject mapManager;
    public int floor = 1;
    private void OnMouseDown()
    {
        mapManager.GetComponent<InteractiveMap>().setFloor(floor);
    }
}
