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
    public int GameTime = 300;
    public GameObject timer;
    private List<Vector3> startPositionItems;
    private GameObject networkManager;
    private MyNetworkManager myNetworkManager;


    //private bool canStartGame = true;
    private void Start()
    {
        networkManager = GameObject.Find("DontDestroyOnLoad/NetworkManager");
        myNetworkManager = networkManager.GetComponent<MyNetworkManager>();
    }

    public void StartGame(GameObject player)
    {
        //player.GetComponent<PlayerNetworkManager>().StartGame(this.gameObject);
        myNetworkManager.ServerChangeScene("Main");
    }

    public void UpdateStartGame()
    {
        //Bad practice we should pass players in some other way 
        players = GameObject.FindGameObjectsWithTag("Player");
        lights.GetComponent<LightManager>().TurnPressureAlarmOn();
        timer.GetComponent<TimerManager>().startTimer(GameTime);
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
