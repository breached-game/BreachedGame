using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class handles breaches. It acts as a timer and once the waterGridTimer has expired, a water breach occurs and a new minigame is created.
public class WaterTimerManager : MonoBehaviour
{
    public float waterGridTimer = 30f;
    public GameObject waterGrid;
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
                gameObject.GetComponent<DropOffMiniGameManager>().Reset();
                waterGrid.SetActive(true);
                run = false;
            }
        }
    }
}
