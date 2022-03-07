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
    [Command]
    public void CmdstartGame()
    {
        //If server only items (this might be true in the future, we can enable them all using this command
        //NetworkServer.SpawnObjects();
        updateStartGame();
    }
    [ClientRpc]
    void updateStartGame()
    {
        //Bad practice we should pass players in some other way 
        players = GameObject.FindGameObjectsWithTag("Player");
        lights.GetComponent<AudioSource>().Play();
        foreach (GameObject player in players)
        {
            player.transform.position = spawnPoint.transform.position;
            playerUI.SetActive(true);
            player.GetComponent<PlayerManager>().TurnOnAudio();
        }
    }
}
