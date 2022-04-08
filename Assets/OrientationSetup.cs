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
            player.transform.position = spawnPoint.transform.position;
            playerUI.SetActive(true);
            //GARBAGE CODING PRACTICE BELOW
            int children = playerUI.transform.childCount;
            for (int i = 0; i < children; i++)
            {
                playerUI.transform.GetChild(i).gameObject.SetActive(true);
            }
            player.GetComponent<PlayerManager>().TurnOnAudio();
        }
        CaptainIntro();
    }

    private void CaptainIntro()
    {
        commandLine.QueueMessage("Welcome to your orientation time");
    }
}
