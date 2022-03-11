using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimmingFishCycle : MonoBehaviour
{
    public GameObject[] patrolPoints;
    public int speed = 5;

    // Update is called once per frame
    void Start()
    {
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            gameObject.transform.LookAt(patrolPoints[i].transform);
            while (patrolPoints[i].transform)
            {
                transform.position += Vector3.forward * Time.deltaTime * speed;
            }
            if (i == patrolPoints.Length) i = 0;
        }
    }
}
