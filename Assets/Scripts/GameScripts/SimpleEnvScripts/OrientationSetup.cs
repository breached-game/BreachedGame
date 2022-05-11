using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class OrientationSetup : MonoBehaviour
{
    /*
        COORDINATES THE SETUP OF THE ORIENTATION SCENE AND THE MINIMAP

        Contributors: Sam Barnes-Thornton and Seth Holdcroft
    */
    public GameObject spawnPoint;
    private GameObject[] players;
    public GameObject playerUI;
    public GameObject lights;
    public GameObject commandObject;
    private CommandManager commandLine;
    public GameObject networkCommand;
    private PlayerNetworkManager playerNetworkManager;
    private GameObject localPlayer;

    public Material localPlayerMinimapColour;
    // Start is called before the first frame update

    private void Awake()
    {
        // Finds all player game objects in the game
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            // Checks if player is local client
            if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                localPlayer = player;
                // Sets the minimap colour for the local player
                player.GetComponent<PlayerManager>().minimapToken.SetActive(true);
                player.GetComponent<PlayerManager>().minimapToken.GetComponent<MeshRenderer>().material = localPlayerMinimapColour;
            }
        }
    }
    void Start()
    {
        commandLine = commandObject.GetComponent<CommandManager>();
        lights.GetComponent<LightManager>().TurnPressureAlarmOff();
        foreach (GameObject player in players)
        {
            playerUI.SetActive(true);
            int children = playerUI.transform.childCount;
            for (int i = 0; i < children; i++)
            {
                playerUI.transform.GetChild(i).gameObject.SetActive(true);
            }
            player.GetComponent<PlayerManager>().TurnOnAudio();
            if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                playerNetworkManager = player.GetComponent<PlayerNetworkManager>();
                playerNetworkManager.StartOrientation();
            }
        }
    }
    private void Update()
    {
        // Fixes problem of players occasionally falling through the map
        if (localPlayer != null)
        {
            if (localPlayer.transform.position.y < 0)
            {
                localPlayer.transform.position = spawnPoint.transform.position;
            }
        }
    }

    // Queues the command line messages to explain the orientation scene
    public void CaptainIntro()
    {
        commandLine.QueueMessage("Welcome to the night shift on HMS Coronation", true);
        commandLine.QueueMessage("There's some food available for you in the kitchen", true);
        commandLine.QueueMessage("I've left the sub on auto-pilot so everyone stay vigilant!", true);
        commandLine.QueueMessage("Use the ladder to go downstairs...", false);
        commandLine.QueueMessage("...then use the mini-map to find the kitchen!", false);
        commandLine.QueueMessage("Press e on your plate when everyone is ready to play", false);
        commandLine.QueueMessage("TIP: Hold down shift to sprint", false);
    }
}
