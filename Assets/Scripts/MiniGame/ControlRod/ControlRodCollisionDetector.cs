using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlRodCollisionDetector : MonoBehaviour
{
    public GameObject manager;

    private void OnTriggerEnter(Collider other)
    {
        manager.GetComponent<ControlRodManager>().Success();
    }
}
