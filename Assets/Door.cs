using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool open = false;
    public void OpenDoor()
    {
        if (!open)
        {
            gameObject.GetComponent<Animator>().Play("DeadBolt");
            Destroy(gameObject.GetComponent<Rigidbody>());
        }
    }
}
