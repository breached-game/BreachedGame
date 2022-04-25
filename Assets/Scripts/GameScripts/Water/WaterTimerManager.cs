using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class handles breaches. It acts as a timer and once the waterGridTimer has expired, a water breach occurs and a new minigame is created.
public class WaterTimerManager : MonoBehaviour
{
    public float waterGridTimer = 30f;
    public GameObject waterGrid;
    public GameObject waterGridWait;
    private bool run = true;

    public bool GetRun()
    {
        return run;
    }

    void Update()
    {
        if (run) {
            if (waterGridTimer > 0)
            {
                waterGridTimer -= Time.deltaTime;
            }
            if (waterGridTimer < 0)
            {
                if (!waterGridWait.activeSelf)
                {
                    gameObject.GetComponent<DropOffMiniGameManager>().Reset();
                    waterGrid.SetActive(true);
                    run = false;
                    gameObject.GetComponent<DropOffMiniGameManager>().minigameManager.commandNetwork.GetComponent<CommandNetworkManager>().SendNetworkMessage("Another breach has opened up due to the pressure, just when we thought we were safe!", true);
                }
            }
        }
    }
}
