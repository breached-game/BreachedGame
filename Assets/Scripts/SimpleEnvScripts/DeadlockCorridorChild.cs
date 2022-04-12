using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlockCorridorChild : MonoBehaviour
{
    public bool DoorOne = false;
    private int numberOfPlayersIn = 0;
    private DeadBoltCorridor deadBoltCorridor;

    private void Start()
    {
        deadBoltCorridor = transform.parent.GetComponent<DeadBoltCorridor>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.tag == "Player")
        {
            numberOfPlayersIn++;
            if (DoorOne)
            {
                deadBoltCorridor.playersIn1 = true;
            } else deadBoltCorridor.playersIn2 = true;
            //Call function to see what door to open
            deadBoltCorridor.OnStateChange();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.tag == "Player")
        {
            numberOfPlayersIn--;
            if (numberOfPlayersIn == 0)
            {
                if (DoorOne)
                {
                    deadBoltCorridor.playersIn1 = false;
                }
                else deadBoltCorridor.playersIn2 = false;
            }
            //Call function to see what door to open
            deadBoltCorridor.OnStateChange();
        }
    }
}
