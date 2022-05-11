using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class StartGameButton : NetworkBehaviour
{
    /*
        SCRIPT ATTACHED TO THE RED BUTTON IN THE LOBBY

        Contributors: Sam Barnes-Thornton
    */
    private GameObject networkManager;
    private MyNetworkManager myNetworkManager;

    private void Start()
    {
        // Sets myNetworkManage variable for all clients so it doesn't have
        // to be found multiple times
        if (!isServer)
        {
            networkManager = GameObject.Find("NetworkManager");
            myNetworkManager = networkManager.GetComponent<MyNetworkManager>();
        }
    }

    public void StartGame(GameObject player)
    {
        // Sets current player as the 'starter' and changes to the orientation scene
        player.GetComponent<PlayerNetworkManager>().ChangeToSub();
    }
}
