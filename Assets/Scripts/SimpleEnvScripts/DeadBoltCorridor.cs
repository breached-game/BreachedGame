using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBoltCorridor : MonoBehaviour
{
    public bool playersIn1 = false;
    public bool playersIn2 = false;
    public bool playersMid = false;
    private int DoorLastOpened = 0;
    private int numberOfPlayersInMid = 0;

    //The doors
    public GameObject door1;
    public GameObject door2;
    //We dont want to play animation if there's not state change
    private bool door1Open = false;
    private bool door2Open = false;

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
                door2Open = false;
            }
        }
        //Opening either way
        if (playersIn1 && !playersMid)
        {
            if (!door1Open)
            {
                door1.GetComponent<Animator>().Play("Open");
                door1Open = true;
            }
            DoorLastOpened = 1;
        }
        else if (playersIn2 && !playersMid)
        {
            if (!door2Open)
            {
                door2.GetComponent<Animator>().Play("Open");
                door2Open = true;
            }
            DoorLastOpened = 2;
        }
        //Openning when a player is in mid
        if (playersMid)
        {
            if (DoorLastOpened == 1)
            {
                if (door1Open)
                {
                    door1.GetComponent<Animator>().Play("Close");
                    door1Open = false;
                    StartCoroutine(waitForDoorAnimation(2));
                }
            }
            else if (DoorLastOpened == 2)
            {
                if (door2Open)
                {
                    door2.GetComponent<Animator>().Play("Close");
                    door2Open = false;
                    StartCoroutine(waitForDoorAnimation(1));
                }
            }
        }
    }
    IEnumerator waitForDoorAnimation(int doorToBeOpen)
    {
        yield return new WaitForSeconds(0.5f);
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