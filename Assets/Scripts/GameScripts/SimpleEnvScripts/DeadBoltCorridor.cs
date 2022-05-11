using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBoltCorridor : MonoBehaviour
{
    /*
        SCRIPT TO MANAGE THE OPENING AND CLOSING OF DEADBOLT DOORS IN CORRIDORS.
        OIGINALLY STOPPED TWO DOORS FROM BEING OPEN AT ONCE BUT THIS CHANGED AS
        IT BECAME TOO COMPLICATED.

        Contributors: Andrew Morgan
    */
    public bool playersIn1 = false;
    public bool playersIn2 = false;
    public bool playersMid = false;
    private int numberOfPlayersInMid = 0;

    // The doors
    public GameObject door1;
    public GameObject door2;
    // The water blocks (empty game objects with rigid bodies)
    public GameObject doorStop1;
    private Vector3 doorStopPos1;
    public GameObject doorStop2;
    private Vector3 doorStopPos2;
    private void Start()
    {
        // Defining vectors for the water stops for the doors
        doorStopPos1 = doorStop1.transform.position;
        doorStopPos2 = doorStop2.transform.position;
    }

    // Called when a player walks near, and therefore needs to
    // open, a door
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.tag == "Player")
        {
            numberOfPlayersInMid++;
            playersMid = true;
            //Call function to see what door to open
            OnStateChange();
        }
    }

    // Called when a player walks away from the door, as it
    // therefore needs to be closed
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.tag == "Player")
        {
            numberOfPlayersInMid--;
            if (numberOfPlayersInMid == 0) playersMid = false;
            //Call function to see what door to open
            OnStateChange();
        }
    }

    // Called by various functions to open the correct doors depending on
    // where players are in the corridor
    public void OnStateChange()
    {
        // Ifs are used rather than else ifs as there could be players on both sides of the corridor
        // Checks whether players are near door 1
        if (playersIn1)
        {
            door1.GetComponent<Animator>().Play("Open");
            // Water stops have to be moved down in order to exit the water collider
            doorStop1.transform.position = new Vector3(doorStopPos1.x, -50, doorStopPos1.z);
        }
        // Checks whether players are near door 2
        if (playersIn2)
        {
            door2.GetComponent<Animator>().Play("Open");
            doorStop2.transform.position = new Vector3(doorStopPos2.x, -50, doorStopPos2.z);
        }
        // Stops players from getting stuck in between the doors
        if (playersMid)
        {
            door1.GetComponent<Animator>().Play("Open");
            door2.GetComponent<Animator>().Play("Open");
            doorStop1.transform.position = new Vector3(doorStopPos1.x, -50, doorStopPos1.z);
            doorStop2.transform.position = new Vector3(doorStopPos2.x, -50, doorStopPos2.z);
        }
        // Closes all doors if there are no players near or in the corridor
        if (!playersIn1 && !playersIn2 && !playersMid)
        {
            door1.GetComponent<Animator>().Play("Close");
            door1.GetComponent<AudioSource>().Play();
            door2.GetComponent<Animator>().Play("Close");
            door2.GetComponent<AudioSource>().Play();
            doorStop2.transform.position = doorStopPos2;
            doorStop1.transform.position = doorStopPos1;
        }
    }
}