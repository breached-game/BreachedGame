using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlRodFailureDetector : MonoBehaviour
{
    /*
     Collider script used to handle failiures in the control rod game
     Contributors : Seth
     */
    public GameObject manager;

    private void OnTriggerEnter(Collider other)
    {
        manager.GetComponent<ControlRodManager>().Failure();
    }
}
