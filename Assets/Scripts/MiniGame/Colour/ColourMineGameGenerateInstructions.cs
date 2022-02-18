using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColourMineGameGenerateInstructions : MonoBehaviour
{
    private List<string> textList;
    private string text;
    // Start is called before the first frame update
    void Start()
    {
        textList = transform.parent.GetComponent<ColourMiniGameManger>().correctColourCombination;
        for (int i = 0; i < textList.Count; i++)
        {
            text += textList[i]+ ", ";
        }
        GetComponent<TextMeshPro>().text = text;      
    }

}
