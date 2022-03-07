using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    public GameObject waterGrid;
    
    public void StartWater()
    {
        waterGrid.GetComponent<WaterGrid>().run = true;
    }
    
    public void StopWater()
    {
        waterGrid.GetComponent<WaterGrid>().run = true;
    }

    public void OutflowWater()
    {
        waterGrid.GetComponent<WaterGrid>().inflowRate = -waterGrid.GetComponent<WaterGrid>().inflowRate;
        waterGrid.GetComponent<WaterGrid>().playerSpeed = 1;
    }
}
