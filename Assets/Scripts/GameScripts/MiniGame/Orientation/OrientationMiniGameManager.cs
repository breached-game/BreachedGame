using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationMiniGameManager : MonoBehaviour
{
    public List<GameObject> playersDonePlate;
    public List<GameObject> playersDoneBed;

    public List<GameObject> beds;

    private GameObject[] players;

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    public void addPlayerPlate(GameObject player)
    {
        playersDonePlate.Add(player);
        if(playersDonePlate.Count == players.Length)
        {
            foreach(GameObject bed in beds)
            {
                bed.GetComponent<BoxCollider>().enabled = true;
            }
        }
    }
    public void addPlayerBed(GameObject player)
    {
        playersDoneBed.Add(player);
        if (playersDonePlate.Count == players.Length)
        {
            //Load cutscene
        }
    }
}
