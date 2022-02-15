using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ColourMiniGameManger : MonoBehaviour
{
    public List<string> correctColourCombination; // Can make this private and randomly generated -> Laterbase
    private List<string> currentColourCombination;

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
            print("Correct combination");
        }
        else
        {
            print("Incorrect combination");
            currentColourCombination = new List<string>();
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
