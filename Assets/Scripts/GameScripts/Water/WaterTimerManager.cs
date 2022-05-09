using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    THIS CLASS HANDLES BREACHES. IT ACTS AS A TIMER AND ONCE THE TIMER HAS EXPIRED, A WATER BREACH OCCURS AND A NEW MINIGAME IS CREATED.
    Contributors: Srdjan Vojnovic
*/
public class WaterTimerManager : MonoBehaviour
{
    public float waterGridTimer = 30f;
    public GameObject waterGrid;
    public GameObject waterGridWait;
    private bool run = true;
    public bool online = true;

    public bool GetRun()
    {
        return run;
    }

    void Update()
    {
        if (run) {
            //This decreases the timer.
            if (waterGridTimer > 0)
            {
                waterGridTimer -= Time.deltaTime;
            }
            if (waterGridTimer < 0)
            {
                if (!waterGridWait.activeSelf)
                {
                    //Sets a new game objective
                    gameObject.GetComponent<DropOffMiniGameManager>().Reset();
                    //Creates new breach
                    waterGrid.SetActive(true);
                    run = false;
                    if (online)
                    {
                        //Sets new breachpoint
                        gameObject.transform.Find("Breach Point").gameObject.SetActive(true);
                        gameObject.transform.Find("Dripping").gameObject.SetActive(true);
                        gameObject.GetComponent<DropOffMiniGameManager>().minigameManager.commandNetwork.GetComponent<CommandNetworkManager>().SendNetworkMessage("Another breach has opened up due to the pressure, just when we thought we were safe!", true);
                    }
                }
            }
        }
    }
}
