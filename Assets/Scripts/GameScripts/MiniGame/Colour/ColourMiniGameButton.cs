using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ColourMiniGameButton : NetworkBehaviour
{
    public string colour;
    public Material mat;
    public void UpdateAllButtonPresses()
    {
        transform.parent.GetComponent<ColourMiniGameManger>().sendPressedColour(colour, mat);
        transform.GetChild(0).GetComponent<Animator>().Play("Click");
    }
}




