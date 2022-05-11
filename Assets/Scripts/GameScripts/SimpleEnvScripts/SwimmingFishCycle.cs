using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimmingFishCycle : MonoBehaviour
{
    /*
        SCRIPT TO ANIMATE THE FISH SWIMMING AROUND IN MENU SCENES AND THE LOBBY

        Contributors: Andrew Morgan
    */

    // Array of colliders to act as bounds for turning the fish around
    public GameObject[] patrolPoints;
    public int speed = 5;
    public int index = 0;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(patrolPoints[index].transform.position);
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    // Checks if fish have hit the bounds of their 'patrol'
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == patrolPoints[index])
        {
            if (index + 1 == patrolPoints.Length) index = 0;
            else index++;
        }
    }
}
