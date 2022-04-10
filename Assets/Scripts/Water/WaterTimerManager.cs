using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTimerManager : MonoBehaviour
{
    public float waterGridTimer = 30f;
    public GameObject waterGrid;

    void Update()
    {
        if (waterGridTimer > 0)
        {
            waterGridTimer -= Time.deltaTime;
        }
        if (waterGridTimer < 0)
        {
            gameObject.GetComponent<DropOffMiniGameManager>().Reset();
            waterGrid.SetActive(true);
        }
    }
}
