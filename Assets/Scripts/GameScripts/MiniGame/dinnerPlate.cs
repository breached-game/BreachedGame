using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dinnerPlate : MonoBehaviour
{
    public List<GameObject> playersDone;
    private GameObject[] players;
    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }
    public void playerDone(GameObject player)
    {
        if(playersDone.Contains(player) == false) playersDone.Add(player);
        if(players.Length == playersDone.Count)
        {
            //Load new scene
        }
    }
}
