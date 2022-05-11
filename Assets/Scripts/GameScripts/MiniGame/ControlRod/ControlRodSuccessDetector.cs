using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlRodSuccessDetector : MonoBehaviour
{
    public GameObject manager;


    //Collider script used to trigger success of control rod minigame
    // Contributors : Seth
    private void OnTriggerEnter(Collider other)
    {
        manager.GetComponent<ControlRodManager>().Success();
 
    }
}
