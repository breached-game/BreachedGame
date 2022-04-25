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
    public int GameTime = 300;
    public GameObject timer;
    public GameObject alarms;
    public GameObject colourManager;
    public GameObject missileTimerText;
    public GameObject commandLine;
    private CommandManager commandLineManager;

    // Start is called before the first frame update
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
                playerManager.StartGame(this.gameObject);
            }
            player.transform.position = spawnPoint.transform.position;
            playerUI.SetActive(true);
            //GARBAGE CODING PRACTICE BELOW
            int children = playerUI.transform.childCount;
            for (int i = 0; i < children; i++)
            {
                playerUI.transform.GetChild(i).gameObject.SetActive(true);
            }
            player.GetComponent<PlayerManager>().TurnOnAudio();
            player.GetComponent<PlayerManager>().torch.SetActive(false);
        }
        commandLineManager = commandLine.GetComponent<CommandManager>();
        commandLineManager.QueueMessage("ACTION STATIONS, ACTION STATIONS!", true);
        commandLineManager.QueueMessage("That sea mine has ripped a hole in the sub and water is flooding in", true);
        commandLineManager.QueueMessage("The loss in pressure has also caused the engines to fail and the reactor to become unstable. Take a look at the monitors to see what needs fixing!", true);
        commandLineManager.QueueMessage("Please work together just like you've trained to stop us from hitting the ocean floor", true);
        commandLineManager.QueueMessage("I'm pretty sure we've got some wooden plugs in one of the store rooms that might help fix the breach...", true);
    }

    public void SetColourCombo(List<string> combination)
    {
        ColourMiniGameManger colourMinigameManager = colourManager.GetComponent<ColourMiniGameManger>();
        colourMinigameManager.correctColourCombination = combination;
        colourMinigameManager.DisplayCombinations();
    }
}
