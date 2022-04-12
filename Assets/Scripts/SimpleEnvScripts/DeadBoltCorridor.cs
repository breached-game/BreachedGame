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
    //We dont want to play animation if there's not state change
    private bool door1Open = false;
    private bool door2Open = false;
    private bool inUse = false;
    private bool oneToTwo = false;

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
                door1.GetComponent<Animator>().Play("Close");
                door1Open = false;
            }
            if (door2Open)
            {
                door2.GetComponent<Animator>().Play("Close");
                door1Open = false;
            }
            inUse = false;
        }
        else if (playersIn1 && !inUse)
        {
            door1.GetComponent<Animator>().Play("Open");
            door1Open = true;
            inUse = true;
            oneToTwo = true;
        }
        else if (playersIn2 && !inUse)
        {
            door2.GetComponent<Animator>().Play("Open");
            door2Open = true;
            inUse = true;
            oneToTwo = false;
        }
        else if(oneToTwo)
        {
            door1.GetComponent<Animator>().Play("Close");
            door1Open = false;
            StartCoroutine(waitForDoorAnimation(2));
        }
        else if (!oneToTwo)
        {
            door2.GetComponent<Animator>().Play("Close");
            door2Open = false;
            StartCoroutine(waitForDoorAnimation(1));
        }
    }
    IEnumerator waitForDoorAnimation(int doorToBeOpen)
    {
        yield return new WaitForSeconds(1f);
        if(doorToBeOpen == 1)
        {
            if (!door1Open)
            {
                door1.GetComponent<Animator>().Play("Open");
                door1Open = true;
            }
        }
        else
        {
            if (!door2Open)
            {
                door2.GetComponent<Animator>().Play("Open");
                door2Open = true;
            }
        }
    }
}