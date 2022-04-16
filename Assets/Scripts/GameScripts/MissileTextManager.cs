using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class MissileTextManager : MonoBehaviour
{
    private Text textDisplay;
    // Start is called before the first frame update
    void Start()
    {
        textDisplay = gameObject.GetComponent<Text>();
        textDisplay.text = "";
    }

    public void UpdateTime(float time)
    {
        textDisplay.text = time.ToString();
    }
}
