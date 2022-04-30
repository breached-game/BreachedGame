using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LobbySetup : MonoBehaviour
{
    public GameObject commandObject;
    private CommandManager commandManager;
    // Start is called before the first frame update

    void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            print("On");
            player.GetComponent<PlayerManager>().TurnOnAudio();
        }
        commandManager = commandObject.GetComponent<CommandManager>();
        commandManager.QueueMessage("Welcome to the lobby area!");
        commandManager.QueueMessage("Please wait until all of your team are in the lobby to press the red button");
        commandManager.QueueMessage("Make sure you have adjusted your brightness using the slider in your settings menu (esc)");
    }
}
