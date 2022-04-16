using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CommandNetworkManager : MonoBehaviour
{
    public GameObject commandObject;
    private CommandManager commandManager;
    private PlayerNetworkManager playerNetworkManager;
    // Start is called before the first frame update
    void Awake()
    {
        commandManager = commandObject.GetComponent<CommandManager>();
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                playerNetworkManager = player.GetComponent<PlayerNetworkManager>();
            }
        }
    }

    public void SendNetworkMessage(string msg, bool captain)
    {
        playerNetworkManager.WriteCommand(this.gameObject, msg, captain);
    }

    public void QueueNetworkMessage(string msg, bool captain)
    {
        print("Msg");
        commandManager.QueueMessage(msg, captain);
    }
}
