using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameButton : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject[] players;
    public GameObject playerUI;

    public void startGame()
    {
        //Bad practice we should pass players in some other way 
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
        {
            player.transform.position = spawnPoint.transform.position;
            playerUI.SetActive(true);
        }
    }
}
