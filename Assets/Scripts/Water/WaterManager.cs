using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WaterManager : NetworkBehaviour
{
    public GameObject waterGridObject;
    private WaterGrid waterGrid;

    private void Start()
    {
        waterGrid = waterGridObject.GetComponent<WaterGrid>();
    }
    public void StartWater()
    {
        waterGrid.run = true;
    }
    
    public void StopWater()
    {
        waterGrid.run = false;
    }

    public void OutflowWater()
    {
        waterGrid.inflowRate = -waterGrid.GetComponent<WaterGrid>().inflowRate;
        waterGrid.pump = true;
    }
}
