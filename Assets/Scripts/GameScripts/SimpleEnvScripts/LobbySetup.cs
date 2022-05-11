using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LobbySetup : MonoBehaviour
{
    /*
        TURNS ON AUDIO AND CALLS COMMAND LINE FOR LOBBY

        Contributors: Sam Barnes-Thornton
    */
    public GameObject commandObject;
    private CommandManager commandManager;

    void Start()
    {
        // Finds all players in the scene and turns on their audio listeners
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            print("On");
            player.GetComponent<PlayerManager>().TurnOnAudio();
        }
        // Queues command line messages for settings etc.
        commandManager = commandObject.GetComponent<CommandManager>();
        commandManager.QueueMessage("Welcome to the lobby area!");
        commandManager.QueueMessage("Please wait until all of your team are in the lobby to press the red button");
        commandManager.QueueMessage("Make sure you have adjusted the music volume using the slider in your settings menu (esc)");
    }
}
