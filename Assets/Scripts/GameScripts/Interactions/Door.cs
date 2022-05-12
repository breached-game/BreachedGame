using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    /*
        OPENS THE DOORS BENEATH THE CONTROL ROOM
        Contributors: Andrew Morgan and Sam Barnes-Thornton
    */

    // Water block to stop water going through the door (unrendered game object with rigid body
    public GameObject doorStop;
    public bool open = false;
    public void OpenDoor()
    {
        if (!open)
        {
            gameObject.GetComponent<Animator>().Play("Open");
            // Water block has to move down for water to go through it
            doorStop.transform.position = new Vector3(doorStop.transform.position.x, -50, doorStop.transform.position.z);
            open = true;
        }
    }
}
