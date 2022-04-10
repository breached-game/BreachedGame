using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class OrientationSetup : MonoBehaviour
{
    public GameObject spawnPoint;
    private GameObject[] players;
    public GameObject playerUI;
    public GameObject lights;
    public GameObject commandObject;
    private CommandManager commandLine;
    public GameObject networkCommand;
    private PlayerNetworkManager playerNetworkManager;
    // Start is called before the first frame update
    void Start()
    {
        commandLine = commandObject.GetComponent<CommandManager>();
        //Bad practice we should pass players in some other way 
        players = GameObject.FindGameObjectsWithTag("Player");
        //lights.GetComponent<LightManager>().TurnPressureAlarmOn();
        lights.GetComponent<LightManager>().TurnPressureAlarmOff();
        foreach (GameObject player in players)
        {
            playerUI.SetActive(true);
            //GARBAGE CODING PRACTICE BELOW
            int children = playerUI.transform.childCount;
            for (int i = 0; i < children; i++)
            {
                playerUI.transform.GetChild(i).gameObject.SetActive(true);
            }
            player.GetComponent<PlayerManager>().TurnOnAudio();
            player.transform.position = spawnPoint.transform.position;
            if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                playerNetworkManager = player.GetComponent<PlayerNetworkManager>();
                playerNetworkManager.StartOrientation();
            }
        }
        playerNetworkManager.WriteCommand(networkCommand, "hello", true);
        CaptainIntro();
    }

    private void CaptainIntro()
    {
        commandLine.QueueMessage("Welcome to the night shift on HMS Coronation", true);
        commandLine.QueueMessage("We have a dangerous area ahead of us which is going to need some careful navigating", true);
        commandLine.QueueMessage("What's going on? You all look half asleep, wandering around like headless chickens!", true);
        commandLine.QueueMessage("You've got one minute to have a stroll around the sub and wake yourselves up for the shift ahead", true);
    }
}
