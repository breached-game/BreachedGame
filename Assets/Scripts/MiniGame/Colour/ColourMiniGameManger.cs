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

    public GameObject combinationDisplay;
    public Material SuccessColour;
    public Material FailColour;
    public float interval = 0.3f;
    public int NumberOfFlashes = 3;

    public void Reset()
    {
        minigameManager = transform.parent.GetComponent<MinigameManager>();
        minigameManager.SendObjectiveData(minigameName, minigameObjective);
    }
    private void Start()
    {
        Reset();
    }

    public void sendPressedColour(string colour, Material mat)
    {
        // Debug.Log("Got colour " + colour);
        currentColourCombination.Add(colour);
        //Display the new colour added
        combinationDisplay.transform.GetChild(currentColourCombination.Count).transform.gameObject.GetComponent<MeshRenderer>().material = mat;
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
            StartCoroutine(FlickerEffect(SuccessColour));
        }
        else
        {
            print("Incorrect combination");
            failiureReason = "Incorrect Combination";
            currentColourCombination = new List<string>();
            minigameManager.ObjectiveFailed(minigameName, failiureReason); //Change this 
            StartCoroutine(FlickerEffect(FailColour));
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

    private void displayCominationOutcome(Material matOutcome)
    {
        for (int i = 0; i < currentColourCombination.Count; i++)
        {
            combinationDisplay.transform.GetChild(currentColourCombination.Count).transform.gameObject.GetComponent<MeshRenderer>().material = matOutcome;
        }
    }

    IEnumerator FlickerEffect(Material matFlicker)
    {
        for (int i = 0; i < NumberOfFlashes; i++)
        {
            displayCominationOutcome(matFlicker);
            yield return new WaitForSeconds(interval);
        }
    }
}
