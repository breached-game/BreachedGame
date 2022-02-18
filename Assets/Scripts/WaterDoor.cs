using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDoor : MonoBehaviour
{
    public GameObject waterGrid;
    // Start is called before the first frame update
    public void StartWater()
    {
        waterGrid.GetComponent<WaterGrid>().run = true;
    }
}
