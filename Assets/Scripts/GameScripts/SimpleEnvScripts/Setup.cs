using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Setup : MonoBehaviour
{
    /*
        IMPLEMENTED IN ORDER TO COMBAT PROBLEMS WITH DESYNCHRONISATION
        DURING NETWORKED SCENE CHANGES. PROVIDES PLAYERNETWORKMANAGER
        WITH VARIOUS GAME OBJECTS FOR LATER USE

        Contributors: Sam Barnes-Thornton
    */
    public GameObject spawnPoint;
    public GameObject[] players;
    public GameObject playerUI;
    public GameObject lights;
    public List<GameObject> items;
    public int GameTime = 480;
    public GameObject timer;
    public GameObject alarms;
    public GameObject colourManager;
    public GameObject missileTimerText;
    public GameObject commandLine;
    private CommandManager commandLineManager;
    private GameObject localPlayer;

    private void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                localPlayer = player;
            }
        }
    }
    void Start()
    {
        //Bad practice we should pass players in some other way 
        players = GameObject.FindGameObjectsWithTag("Player");
        //lights.GetComponent<LightManager>().TurnPressureAlarmOn();
        timer.GetComponent<TimerManager>().startTimer(GameTime);
        lights.GetComponent<LightManager>().TurnPressureAlarmOff();
        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                PlayerNetworkManager playerManager = player.GetComponent<PlayerNetworkManager>();
                player.GetComponent<PlayerManager>().TurnOnAudio();
                player.GetComponent<PlayerManager>().torch.SetActive(false);
                player.transform.position = spawnPoint.transform.position;
                // Calls StartGame in PlayerNetworkManager to setup timers and alarms etc.
                playerManager.StartGame(this.gameObject);
                break;
            }
        }
        playerUI.SetActive(true);
        int children = playerUI.transform.childCount;
        for (int i = 0; i < children; i++)
        {
            playerUI.transform.GetChild(i).gameObject.SetActive(true);
        }
        // Uses command line to setup the story for the game
        commandLineManager = commandLine.GetComponent<CommandManager>();
        commandLineManager.QueueMessage("ACTION STATIONS, ACTION STATIONS!", true);
        commandLineManager.QueueMessage("That sea mine has ripped a hole in the sub and water is flooding in!", true);
        commandLineManager.QueueMessage("Use the monitors to see what needs fixing", true);
        commandLineManager.QueueMessage("Get the breach fixed first so it's easier to move around", true);
        commandLineManager.QueueMessage("I'm pretty sure theres some wooden breach plugs in the stores...", true);
    }

    // Called by PlayerNetworkManager to set the random colour combination for the missiles
    public void SetColourCombo(List<string> combination)
    {
        ColourMiniGameManger colourMinigameManager = colourManager.GetComponent<ColourMiniGameManger>();
        colourMinigameManager.correctColourCombination = combination;
        colourMinigameManager.DisplayCombinations();
    }

    private void Update()
    {
        // Used to combat strange bug where players fall through map after spawning
        if (localPlayer != null)
        {
            if (localPlayer.transform.position.y < 0)
            {
                localPlayer.transform.position = spawnPoint.transform.position;
            }
        }
    }
}
