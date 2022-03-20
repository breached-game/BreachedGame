using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class StartGameButton : NetworkBehaviour
{
    /*
    public GameObject spawnPoint;
    public GameObject[] players;
    public GameObject playerUI;
    public GameObject lights;
    public List<GameObject> items;
    public int GameTime = 300;
    public GameObject timer;
    private List<Vector3> startPositionItems;
    */
    private GameObject networkManager;
    private MyNetworkManager myNetworkManager;


    //private bool canStartGame = true;
    private void Start()
    {
        if (!isServer)
        {
            networkManager = GameObject.Find("NetworkManager");
            myNetworkManager = networkManager.GetComponent<MyNetworkManager>();
        }
    }

    public void StartGame(GameObject player)
    {
        player.GetComponent<PlayerNetworkManager>().ChangeToSub();
    }

    /*
    public void UpdateStartGame()
    {
        //Bad practice we should pass players in some other way 
        players = GameObject.FindGameObjectsWithTag("Player");
        lights.GetComponent<LightManager>().TurnPressureAlarmOn();
        timer.GetComponent<TimerManager>().startTimer(GameTime);
        for(int i =0; i < players.Length; i++)
        {
            players[i].transform.position = spawnPoint.transform.GetChild(i).transform.position;
            playerUI.SetActive(true);
            //GARBAGE CODING PRACTICE BELOW
            int children = playerUI.transform.childCount;
            for (int a = 0; a < children; a++)
            {
                playerUI.transform.GetChild(a).gameObject.SetActive(true);
            }
            players[i].GetComponent<PlayerManager>().TurnOnAudio();
        }
        foreach(GameObject item in items)
        {
            item.SetActive(true);
        }
    }
    */
}
