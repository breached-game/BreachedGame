using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class MinigameManager : MonoBehaviour
{
  
    public GameObject UIManager;
    private string currentText;


    // Dictionary shape : {Objective name (string) : Instructions (array of string) 
    public Dictionary<string, string> allObjectives = new Dictionary<string, string>();
    private Dictionary<string, string> currentObjectives;
   

    // Dictionary shape : {Objective name (string) : Reason for failiuew (string)
    private Dictionary<string, string> failedObjectives;



    public void SendObjectiveData(string objectiveName, string objectiveDescription)
    {

        //Debug.Log(objectiveName + "\n" + objectiveDescription);
        allObjectives.Add(objectiveName, objectiveDescription);
        currentObjectives = allObjectives;
        UpdateObjectivesPlayerUI();
    }

    public void ObjectiveCompleted(string objectiveName)
    {
        //Called when minigame finished. Removes entry from current objectives, checks if won, updates players UI
        currentObjectives.Remove(objectiveName);
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
            // Win !!!!
        }
    }


    public void UpdateObjectivesPlayerUI()
    {
        //UIManager.GetComponent<PlayerUIManager>().updateObjectiveList(currentObjectives);
    }
}
