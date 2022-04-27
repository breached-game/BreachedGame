using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Setup : MonoBehaviour
{
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

    // Start is called before the first frame update

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
        print(timer);
        print(lights);
        print(alarms);
        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                PlayerNetworkManager playerManager = player.GetComponent<PlayerNetworkManager>();
                playerManager.UpdateStartGame(this.gameObject);
                player.GetComponent<PlayerManager>().TurnOnAudio();
                player.GetComponent<PlayerManager>().torch.SetActive(false);
                player.transform.position = spawnPoint.transform.position;
                playerManager.StartGame(this.gameObject);
                break;
            }
        }
        playerUI.SetActive(true);
        //GARBAGE CODING PRACTICE BELOW
        int children = playerUI.transform.childCount;
        for (int i = 0; i < children; i++)
        {
            playerUI.transform.GetChild(i).gameObject.SetActive(true);
        }
        commandLineManager = commandLine.GetComponent<CommandManager>();
        commandLineManager.QueueMessage("ACTION STATIONS, ACTION STATIONS!", true);
        commandLineManager.QueueMessage("That sea mine has ripped a hole in the sub and water is flooding in", true);
        commandLineManager.QueueMessage("Use the monitors to see what needs fixing", true);
        commandLineManager.QueueMessage("Get the breahc fixed first so it's easier to move around", true);
        commandLineManager.QueueMessage("I'm pretty sure theres some wooden breach plugs in the stores", true);
    }

    public void SetColourCombo(List<string> combination)
    {
        ColourMiniGameManger colourMinigameManager = colourManager.GetComponent<ColourMiniGameManger>();
        colourMinigameManager.correctColourCombination = combination;
        colourMinigameManager.DisplayCombinations();
    }

    private void Update()
    {
        if (localPlayer != null)
        {
            if (localPlayer.transform.position.y < 0)
            {
                localPlayer.transform.position = spawnPoint.transform.position;
            }
        }
    }
}
