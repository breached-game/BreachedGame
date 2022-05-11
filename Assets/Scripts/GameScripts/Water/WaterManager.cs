using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/*
THIS CLASS HANDLES ALL OBJECTS WHICH AFFECT WATER. THESE INCLUDE WOOD AND WATER PUMPS.
Contributors: Srdjan Vojnovic
*/

public class WaterManager : NetworkBehaviour
{
    public GameObject[] waterGridObjects;
    private List<WaterGrid> waterGrids = new List<WaterGrid>();

    private void Start()
    {
        //Initialises water grids
        for (int i = 0; i < waterGridObjects.Length; i++)
        {
            waterGrids.Add(waterGridObjects[i].GetComponent<WaterGrid>());
        }
    }
    public void StartWater()
    {
        //waterGrid.run = true;
    }
    
    //Calls stop breach in water grid, which stops water to inflow.
    public void StopBreach()
    {
        Vector3Int breachPosition;
        for (int i = 0; i < waterGridObjects.Length; i++)
        {
            if (waterGridObjects[i].activeSelf)
            {
                breachPosition = waterGrids[i].water_grid.LocalToCell(transform.position - waterGrids[i].water_grid.transform.position);
                if (breachPosition.x >= 0 & breachPosition.x < waterGrids[i].width)
                {
                    if (breachPosition.y >= 0 & breachPosition.y < waterGrids[i].height)
                    {
                        if (breachPosition.z >= 0 & breachPosition.z < waterGrids[i].depth)
                        {
                            waterGrids[i].StopBreach();
                        }
                    }
                }
            }
        }
    }

    //Adds outflow in water grid
    public void AddPump()
    {
        Vector3Int waterPumpGridPosition;
        for (int i = 0; i < waterGridObjects.Length; i++)
        {
            if (waterGridObjects[i].activeSelf)
            {
                waterPumpGridPosition = waterGrids[i].water_grid.LocalToCell(transform.position - waterGrids[i].water_grid.transform.position);
                if (waterPumpGridPosition.x >= 0 & waterPumpGridPosition.x < waterGrids[i].width)
                {
                    if (waterPumpGridPosition.y >= 0 & waterPumpGridPosition.y < waterGrids[i].height)
                    {
                        if (waterPumpGridPosition.z >= 0 & waterPumpGridPosition.z < waterGrids[i].depth)
                        {
                            waterGrids[i].AddWaterPump(transform.position);
                            transform.GetChild(0).GetComponent<Animator>().enabled = true;
                        }
                    }
                }
            }
        }
    }

    //Removes outflow in water grid.
    public void RemovePump()
    {
        Vector3Int waterPumpGridPosition;

        for (int i = 0; i < waterGridObjects.Length; i++)
        {
            if (waterGridObjects[i].activeSelf)
            {
                waterPumpGridPosition = waterGrids[i].water_grid.LocalToCell(transform.position - waterGrids[i].water_grid.transform.position);
                if (waterPumpGridPosition.x >= 0 & waterPumpGridPosition.x < waterGrids[i].width)
                {
                    if (waterPumpGridPosition.y >= 0 & waterPumpGridPosition.y < waterGrids[i].height)
                    {
                        if (waterPumpGridPosition.z >= 0 & waterPumpGridPosition.z < waterGrids[i].depth)
                        {
                            waterGrids[i].RemoveWaterPump(transform.position);
                            transform.GetChild(0).GetComponent<Animator>().enabled = false;
                        }
                    }
                }
            }
        }
    }
}
