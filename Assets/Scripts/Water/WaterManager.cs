using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WaterManager : NetworkBehaviour
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

    [Command]
    public void CmdOutflowWater()
    {
        OutflowWater();
    }

    [ClientRpc]
    public void OutflowWater()
    {
        waterGrid.GetComponent<WaterGrid>().inflowRate = -waterGrid.GetComponent<WaterGrid>().inflowRate;
        waterGrid.GetComponent<WaterGrid>().playerSpeed = 1;
    }
}
