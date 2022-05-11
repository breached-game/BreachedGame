using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject doorStop;
    public bool open = false;
    public void OpenDoor()
    {
        if (!open)
        {
            gameObject.GetComponent<Animator>().Play("Open");
            doorStop.transform.position = new Vector3(doorStop.transform.position.x, -50, doorStop.transform.position.z);
            open = true;
        }
    }
}
