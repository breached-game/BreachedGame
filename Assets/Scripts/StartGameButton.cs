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
    public int GameTime = 500;
    public GameObject timer;
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
    public void StartGame(GameObject player)
    {
        player.GetComponent<PlayerNetworkManager>().StartGame(lights, timer, playerUI, spawnPoint, GameTime, items.ToArray());
    }
}
