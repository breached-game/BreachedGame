using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class ColourMiniGameManger : MonoBehaviour
{
    public List<string> correctColourCombination; // Can make this private and randomly generated -> Laterbase
    private List<string> currentColourCombination = new List<string>();

    private MinigameManager minigameManager;

    public string minigameName;
    [TextArea]
    public string minigameObjective;

    private string failiureReason;

    public GameObject combinationDisplay;
    public Material SuccessColour;
    public Material FailColour;
    public Material white;
    public float interval = 0.3f;
    public int NumberOfFlashes = 3;
    public GameObject[] players;

    public Material red;
    public Material blue;
    public Material green;

    public GameObject combinationDisplayParent;

    public void Reset()
    {
        minigameManager = transform.parent.GetComponent<MinigameManager>();
        minigameManager.SendObjectiveData(minigameName, minigameObjective);
    }

    public void DisplayCombinations()
    {
        int count = 0;
        Material mat = red;
        foreach(Transform child in combinationDisplayParent.transform)
        {
            var combination_i = correctColourCombination[count];
            if      (combination_i == "red")  {mat = red;}
            else if (combination_i == "blue") {mat = blue;}
            else if (combination_i == "green"){mat = green;}
            else
            {
                print("Error no colour " + combination_i);
            }

            child.transform.gameObject.GetComponent<MeshRenderer>().material = mat;
            count += 1;
        }
    }

    private void Start()
    {
        Reset();
        DisplayCombinations();
    }

    public void sendPressedColour(string colour, Material mat)
    {
        //We check so see if we can add more to the combination
        if (correctColourCombination.Count != currentColourCombination.Count)
        {
            // Debug.Log("Got colour " + colour);
            currentColourCombination.Add(colour);
            //Display the new colour added
            combinationDisplay.transform.GetChild(currentColourCombination.Count - 1).transform.gameObject.GetComponent<MeshRenderer>().material = mat;
            //Debug.Log(currentColourCombination + "\n" + correctColourCombination);
            if (correctColourCombination.Count == currentColourCombination.Count)
            {
                checkCombination();
            }
        }
    }

    public void checkCombination()
    {
        if (listAreEqual(currentColourCombination,correctColourCombination))
        {
            StartCoroutine(FlickerEffect(SuccessColour));
            //print("Correct combination");
            minigameManager.ObjectiveCompleted(minigameName, minigameObjective);
            players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    player.GetComponent<PlayerNetworkManager>().ChangeToVictory();
                }
            }
        }
        else
        {
            StartCoroutine(FlickerEffect(FailColour));
            print("Incorrect combination");
            failiureReason = "Incorrect Combination";
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

    private void displayCominationOutcome(Material matOutcome)
    {
        for (int i = 0; i < currentColourCombination.Count; i++)
        {
            combinationDisplay.transform.GetChild(i).transform.gameObject.GetComponent<MeshRenderer>().material = matOutcome;
        }
    }

    IEnumerator FlickerEffect(Material matFlicker)
    {
        for (int i = 0; i <= NumberOfFlashes; i++)
        {
            displayCominationOutcome(matFlicker);
            yield return new WaitForSeconds(interval);
            displayCominationOutcome(white);
            yield return new WaitForSeconds(interval);
        }
        //RESET
        displayCominationOutcome(white);
        currentColourCombination = new List<string>();
        print("All done");
    }
}
