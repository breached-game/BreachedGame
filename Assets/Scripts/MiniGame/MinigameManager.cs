using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class MinigameManager : MonoBehaviour
{
    public GameObject winScreen;//
    private GameObject[] players;
    
    public GameObject UIManager;
    public GameObject ConsoleLog;
    private string currentText;


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
    }

    public void ObjectiveFailed(string objectiveName, string reasonForFailiure)
    {
        //Outputs failiure (and reason?) to player canvas

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
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(currentObjectives.Count);
        List<string> objectiveNames = new List<string>();
        List<string> objectiveDescriptions = new List<string>();
        List<string> completedObjectiveNames = new List<string>();
        List<string> completedObjectiveDescriptions = new List<string>();
        foreach (var o in currentObjectives)
        {
            objectiveNames.Add(o.Key);
            objectiveDescriptions.Add(o.Value);
        }
        foreach (var o in doneObjectives)
        {
            completedObjectiveNames.Add(o.Key);
            completedObjectiveDescriptions.Add(o.Value);
        }
        foreach (GameObject p in players)
        {
            if (p.GetComponent<PlayerManager>().identity.isLocalPlayer)
            {
                p.GetComponent<PlayerManager>().CmdUpdateObjectiveList(objectiveNames.ToArray(), objectiveDescriptions.ToArray(), completedObjectiveNames.ToArray(), completedObjectiveDescriptions.ToArray());

            }
        }
    }
}
