using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourMiniGameButton : MonoBehaviour
{
    public string colour;

    public void buttonPressed()
    { 
        transform.parent.GetComponent<ColourMiniGameManger>().sendPressedColour(colour);
    }
}

