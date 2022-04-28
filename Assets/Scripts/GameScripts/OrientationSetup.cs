using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private GameObject localPlayer;

    public Material localPlayerMinimapColour;
    // Start is called before the first frame update

    private void Awake()
    {
       players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                localPlayer = player;
                player.GetComponent<PlayerManager>().minimapToken.GetComponent<MeshRenderer>().material = localPlayerMinimapColour;
            }
        }
    }
    void Start()
    {
        commandLine = commandObject.GetComponent<CommandManager>();
        //Bad practice we should pass players in some other way 
        
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
            if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                playerNetworkManager = player.GetComponent<PlayerNetworkManager>();
                playerNetworkManager.StartOrientation();
            }
        }
        CaptainIntro();
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
    private void CaptainIntro()
    {
        commandLine.QueueMessage("Welcome to the night shift on HMS Coronation", true);
        commandLine.QueueMessage("There's some food available for you in the kitchen", true);
        commandLine.QueueMessage("Use the mini-map to go and grab it", true);
        commandLine.QueueMessage("I've left the sub on auto-pilto so hopefully everything goes smoothly", true);
    }
}
