using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class StartGameButton : NetworkBehaviour
{
    public GameObject spawnPoint;
    public GameObject[] players;
    public GameObject playerUI;
    public GameObject lights;
    public List<GameObject> items;
    private List<Vector3> startPositionItems;

    //private bool canStartGame = true;
    private void Start()
    {
       /* if (isServer)
        {
            foreach(GameObject item in items)
            {
                startPositionItems.Add(item.transform.position);
            }
        }*/
    }

    [Command]
    public void CmdstartGame()
    {
        //If server only items (this might be true in the future, we can enable them all using this command
        //NetworkServer.SpawnObjects();
        /*
        int index = 0;
        foreach (GameObject item in items)
        {
            item.transform.position = startPositionItems[index];
            index++;
        }
        */
        updateStartGame();
    }
    [ClientRpc]
    void updateStartGame()
    {
        //Bad practice we should pass players in some other way 
        players = GameObject.FindGameObjectsWithTag("Player");
        lights.GetComponent<LightManager>().TurnPressureAlarmOn();
        foreach (GameObject player in players)
        {
            player.transform.position = spawnPoint.transform.position;
            playerUI.SetActive(true);
            //GARBAGE CODING PRACTICE BELOW
            int children = playerUI.transform.childCount;
            for (int i = 0; i < children; i++)
            {
                playerUI.transform.GetChild(i).gameObject.SetActive(true);
            }
            player.GetComponent<PlayerManager>().TurnOnAudio();
        }
        foreach(GameObject item in items)
        {
            item.SetActive(true);
        }
    }
}
