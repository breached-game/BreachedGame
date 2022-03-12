using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlRodSuccessDetector : MonoBehaviour
{
    public GameObject manager;



    private void OnTriggerEnter(Collider other)
    {
        manager.GetComponent<ControlRodManager>().Success();
 
    }
}
