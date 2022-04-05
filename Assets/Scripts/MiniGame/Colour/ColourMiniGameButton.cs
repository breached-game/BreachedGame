using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ColourMiniGameButton : NetworkBehaviour
{
    public string colour;
    public Material mat;

    private ColourMiniGameManger colourMiniGameManger;

    public void Start()
    {
        colourMiniGameManger = transform.parent.GetComponent<ColourMiniGameManger>();
    }
    public void UpdateAllButtonPresses()
    {
        if (colourMiniGameManger.began == true) 
        {
            colourMiniGameManger.sendPressedColour(colour, mat);
            print("We syncing the button y'all");
            transform.GetChild(0).GetComponent<Animator>().Play("Click");
        }
    }
}




