using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayerObjectiveStatus : MonoBehaviour
{
    // NOT USING THIS ANYMORE

    public Text ObjectiveStatusDisplay;
    public Text FailiureReasonDisplay;


    private string successText = "Objective Completed: ";
    private string failText = "Objective Failed: ";
   
    public void Success(string objectiveName)
    {
        ObjectiveStatusDisplay.color = Color.green;
        ObjectiveStatusDisplay.text = successText + objectiveName;
   

    }

    public void Fail(string objectiveName, string failiureReason)
    {

        ObjectiveStatusDisplay.color = Color.red;
        ObjectiveStatusDisplay.text = failText + objectiveName;

        FailiureReasonDisplay.color = Color.red;
        FailiureReasonDisplay.text = failiureReason;

       

    }

  

}
