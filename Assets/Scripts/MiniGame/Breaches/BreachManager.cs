using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreachManager : MonoBehaviour
{
    public GameObject[] waterGrids;

    public void Start()
    {
        int numBreaches = 0;
        for (int i = 0; i < waterGrids.Length; i++)
        {
            if (waterGrids[i].active)
            {
                numBreaches++;
            }
        }

        gameObject.GetComponent<DropOffMiniGameManager>().numberOfDropOffs = numBreaches;
    }
}
