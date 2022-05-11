using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlockCorridorChild : MonoBehaviour
{
    /*
        SCRIPT ATTACHED TO EACH INDIVIDUAL DOOR IN THE DEAD
        LOCK CORRIDORS. USED TO ALERT THE CORRIDOR SCRIPT 
        WHEN PLAYERS ARE NEAR A SPECIFIC DOOR

        Contributors: Andrew Morgan
    */
    public bool DoorOne = false;
    private int numberOfPlayersIn = 0;
    private DeadBoltCorridor deadBoltCorridor;

    private void Start()
    {
        // Sets Unity object reference for later use
        deadBoltCorridor = transform.parent.GetComponent<DeadBoltCorridor>();
    }

    // Checks whether player is in collider
    // Then performs necessary actions to open doors
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.tag == "Player")
        {
            numberOfPlayersIn++;
            // Script is either attached to door one or door two in a corridor
            if (DoorOne)
            {
                deadBoltCorridor.playersIn1 = true;
            } else deadBoltCorridor.playersIn2 = true;
            //Call corridor function to see what door to open
            deadBoltCorridor.OnStateChange();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.tag == "Player")
        {
            // One less player is in the collider
            numberOfPlayersIn--;
            // Only changes the state of the doors if there are no players left in the collider
            if (numberOfPlayersIn == 0)
            {
                // Script is either attached to door one or door two in a corridor
                if (DoorOne)
                {
                    deadBoltCorridor.playersIn1 = false;
                }
                else deadBoltCorridor.playersIn2 = false;
            }
            //Call corridor function to see what door to open
            deadBoltCorridor.OnStateChange();
        }
    }
}
