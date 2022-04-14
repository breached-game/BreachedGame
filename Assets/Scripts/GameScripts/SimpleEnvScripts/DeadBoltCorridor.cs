using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBoltCorridor : MonoBehaviour
{
    //This is horrible code, I'm actually sorry :( -Andrew
    public bool playersIn1 = false;
    public bool playersIn2 = false;
    public bool playersMid = false;
    private int numberOfPlayersInMid = 0;

    //The doors
    public GameObject door1;
    public GameObject door2;
    public GameObject doorStop1;
    private Vector3 doorStopPos1;
    public GameObject doorStop2;
    private Vector3 doorStopPos2;
    //We dont want to play animation if there's not state change
    private bool door1Open = false;
    private bool door2Open = false;
    private bool inUse = false;
    private bool oneToTwo = false;
    private void Start()
    {
        doorStopPos1 = doorStop1.transform.position;
        doorStopPos2 = doorStop2.transform.position;
    }
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
    public void OnStateChange()
    {
        if(!playersIn1 && !playersIn2 && !playersMid)
        {
            if (door1Open)
            {
                StartCoroutine(waitForCloseAnimation(1));
            }
            if (door2Open)
            {
                StartCoroutine(waitForCloseAnimation(2));
            }
            inUse = false;
        }
        else if (playersIn1 && !inUse)
        {
            door1.GetComponent<Animator>().Play("Open");
            doorStop1.transform.position = new Vector3(doorStopPos1.x, -50, doorStopPos1.z);
            door1Open = true;
            inUse = true;
            oneToTwo = true;
        }
        else if (playersIn2 && !inUse)
        {
            door2.GetComponent<Animator>().Play("Open");
            doorStop2.transform.position = new Vector3(doorStopPos2.x, -50, doorStopPos2.z);
            door2Open = true;
            inUse = true;
            oneToTwo = false;
        }
        else if(oneToTwo)
        {
            StartCoroutine(waitForCloseAnimation(1));
            StartCoroutine(waitForDoorAnimation(2));
            //doorStop1.transform.position = doorStopPos1;
        }
        else if (!oneToTwo)
        {
            StartCoroutine(waitForCloseAnimation(2));
            StartCoroutine(waitForDoorAnimation(1));
            //doorStop2.transform.position = doorStopPos2;
        }
    }
    IEnumerator waitForDoorAnimation(int doorToBeOpen)
    {
        yield return new WaitForSeconds(2f);
        if(doorToBeOpen == 1)
        {
            if (!door1Open)
            {
                door1.GetComponent<Animator>().Play("Open");
                doorStop1.transform.position = new Vector3(doorStopPos1.x, -50, doorStopPos1.z);
                door1Open = true;
            }
        }
        else
        {
            if (!door2Open)
            {
                door2.GetComponent<Animator>().Play("Open");
                doorStop2.transform.position = new Vector3(doorStopPos2.x, -50, doorStopPos2.z);
                door2Open = true;
            }
        }
    }
    IEnumerator waitForCloseAnimation(int doorToBeClosed)
    {
        yield return new WaitForSeconds(1f);
        if(doorToBeClosed == 1)
        {
            if (door1Open)
            {
                door1.GetComponent<Animator>().Play("Close");
                doorStop1.transform.position = doorStopPos1;
                door1Open = false;
            }
        }
        else
        {
            if (door2Open)
            {
                door2.GetComponent<Animator>().Play("Close");
                doorStop2.transform.position = doorStopPos2;
                door2Open = false;
            }
        }
    }
}