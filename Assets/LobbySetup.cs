using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySetup : MonoBehaviour
{
    public GameObject commandObject;
    private CommandManager commandManager;
    // Start is called before the first frame update
    void Start()
    {
        commandManager = commandObject.GetComponent<CommandManager>();
        commandManager.QueueMessage("Welcome to the lobby area!");
        commandManager.QueueMessage("Press that red button when your whole team is in the lobby");
        commandManager.QueueMessage("Make sure you have adjusted your brightness using the slider in your settings menu (esc)");
    }
}
