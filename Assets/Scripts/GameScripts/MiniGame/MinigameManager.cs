using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using Mirror;

public class MinigameManager : MonoBehaviour
{
    public GameObject winScreen; //Not used
    private GameObject[] players;
    
    public GameObject UIManager;

    private bool endgame = false;
    public GameObject ObjectiveStatusUI; //Only set active
    public GameObject ObjectiveCompleteGif;
    public Text ObjectiveStatusDisplay;
    public Text FailiureReasonDisplay;
    public int objectiveStatusPopUpTime;
    public GameObject commandLine;
    public GameObject Caps;
    public GameObject commandNetwork;


    // Dictionary shape : {Objective name (string) : Instructions (string) 
    public Dictionary<string, string> allObjectives = new Dictionary<string, string>();
    private Dictionary<string, string> currentObjectives = new Dictionary<string, string>();
    private Dictionary<string, string> doneObjectives = new Dictionary<string, string>();


    //Testing reasons
    public Dictionary<string, string> GetCurrentObjectives()
    {
        return currentObjectives;
    }

    //Testing reasons
    public Dictionary<string, string> GetDoneObjectives()
    {
        return doneObjectives;
    }

    //Testing reasons
    public bool GetEndgame()
    {
        return endgame;
    }

    private void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Dictionary shape : {Objective name (string) : Reason for failure (string)
    private Dictionary<string, string> failedObjectives;

    public void SendObjectiveData(string objectiveName, string objectiveDescription)
    {
        //Debug.Log(objectiveName + "\n" + objectiveDescription);
        allObjectives.Add(objectiveName, objectiveDescription);
        currentObjectives = allObjectives;
        UpdateObjectivesPlayerUI();
    }

    public void ObjectiveCompleted(string objectiveName, string objectiveDescription)
    {
        //ConsoleLog.GetComponent<ConsoleLog>().createLogItem("Mini Game Completed: " + objectiveName);
        //Called when minigame finished. Removes entry from current objectives, checks if won, updates players UI
        currentObjectives.Remove(objectiveName);
        //Add it to done objectives
        doneObjectives.Add(objectiveName, objectiveDescription);
        CheckWon();
        UpdateObjectivesPlayerUI();
        ShowSuccess(objectiveName);
        commandLine.GetComponent<CommandManager>().QueueMessage("Well done you solved one problem: " + objectiveName, true);
    }

    public void ObjectiveFailed(string objectiveName, string reasonForFailiure)
    {
        //Outputs failiure (and reason?) to player canvas

        ShowFail(objectiveName, reasonForFailiure);

    }
    private string failText = "Objective Failed: ";

    public void ShowSuccess(string objectiveName)
    {
        ObjectiveCompleteGif.SetActive(true);
        StartCoroutine(TextDisplayTime());
        ObjectiveCompleteGif.GetComponent<UISpritesAnimation>().Play();
    }

    public void ShowFail(string objectiveName, string failiureReason)
    {
        FailiureReasonDisplay.enabled = true;

        ObjectiveStatusDisplay.color = Color.red;
        ObjectiveStatusDisplay.text = failText + objectiveName;

        FailiureReasonDisplay.color = Color.red;
        FailiureReasonDisplay.text = failiureReason;
        StartCoroutine(TextDisplayTime());
    }

    IEnumerator TextDisplayTime()
    {
        ObjectiveStatusUI.SetActive(true);
        yield return new WaitForSeconds(objectiveStatusPopUpTime);
        ObjectiveStatusUI.SetActive(false);

    }

    public void CheckWon()
    {
        if (currentObjectives.Count == 1)
        {
            if (!endgame)
            {
                print("endgame");

                endgame = true;
                commandLine.GetComponent<CommandManager>().QueueMessage("Damn it! Resetting the systems has set off the missile launch protocol", true);
                commandLine.GetComponent<CommandManager>().QueueMessage("You've got 60 seconds to input the code from the command room", true);
                Caps.SetActive(false);
                foreach (GameObject player in players)
                {
                    if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
                    {
                        player.GetComponent<PlayerNetworkManager>().StartMissileTimer();
                    }
                }
            }
        }
    }

    public void UpdateObjectivesPlayerUI()
    {
        UIManager.GetComponent<PlayerUIManager>().updateObjectiveList(currentObjectives, doneObjectives);
    }
}
