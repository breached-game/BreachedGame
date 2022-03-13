using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    public GameObject winScreen;//
    private GameObject[] players;
    
    public GameObject UIManager;


    public GameObject ObjectiveStatusUI;
    public Text ObjectiveStatusDisplay;
    public Text FailiureReasonDisplay;
    public int objectiveStatusPopUpTime;


    // Dictionary shape : {Objective name (string) : Instructions (string) 
    public Dictionary<string, string> allObjectives = new Dictionary<string, string>();
    private Dictionary<string, string> currentObjectives = new Dictionary<string, string>();
    private Dictionary<string, string> doneObjectives = new Dictionary<string, string>();


    // Dictionary shape : {Objective name (string) : Reason for failiuew (string)
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
    }

    public void ObjectiveFailed(string objectiveName, string reasonForFailiure)
    {
        //Outputs failiure (and reason?) to player canvas

        ShowFail(objectiveName, reasonForFailiure);

    }
    private string successText = "Objective Completed: ";
    private string failText = "Objective Failed: ";

    public void ShowSuccess(string objectiveName)
    {
        FailiureReasonDisplay.enabled = false;

        ObjectiveStatusDisplay.color = Color.green;
        ObjectiveStatusDisplay.text = successText + objectiveName;
        StartCoroutine(TextDisplayTime());
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
        if (currentObjectives.Count == 0)
        {

            //This function should probably be its own script
            players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerManager>().TurnOffAudio();
            }

            UIManager.SetActive(false);
            winScreen.SetActive(true);
            // Win !!!!
        }
    }


    public void UpdateObjectivesPlayerUI()
    {
        UIManager.GetComponent<PlayerUIManager>().updateObjectiveList(currentObjectives, doneObjectives);
    }
}
