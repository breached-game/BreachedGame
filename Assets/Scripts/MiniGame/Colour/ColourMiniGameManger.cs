using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ColourMiniGameManger : MonoBehaviour
{
    public List<string> correctColourCombination; // Can make this private and randomly generated -> Laterbase
    private List<string> currentColourCombination = new List<string>();

    private MinigameManager minigameManager;

    public string minigameName;
    public string minigameObjective;

    private string failiureReason;

    private void Start()
    {
        minigameManager = transform.parent.GetComponent<MinigameManager>();
        minigameManager.SendObjectiveData(minigameName, minigameObjective);
    }

    public void sendPressedColour(string colour)
    {
        // Debug.Log("Got colour " + colour);
        currentColourCombination.Add(colour);
        //Debug.Log(currentColourCombination + "\n" + correctColourCombination);
        if (correctColourCombination.Count == currentColourCombination.Count)
        {
            checkCombination();
        }

    }

    public void checkCombination()
    {
        if (listAreEqual(currentColourCombination,correctColourCombination))
        {
            //print("Correct combination");
            minigameManager.ObjectiveCompleted(minigameName, minigameObjective); 
        }
        else
        {
            print("Incorrect combination");
            failiureReason = "Incorrect Combination";
            currentColourCombination = new List<string>();
            minigameManager.ObjectiveFailed(minigameName, failiureReason); //Change this 
        }
    }

    private bool listAreEqual(List<string> l1, List<string> l2)
    {
        for (int i = 0; i < l1.Count; i++)
        {
            if (l1[i] != l2[i])
                return false;
        }
        return true;
    }
}
