using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ColourMiniGameButton : NetworkBehaviour
{
    public string colour;
    public Material mat;

    [Command]
    public void CmdButtonPressed()
    {
        UpdateAllButtonPresses();
        transform.parent.GetComponent<ColourMiniGameManger>().sendPressedColour(colour, mat);
        print("We syncing the button y'all");
        transform.GetChild(0).GetComponent<Animator>().Play("Click");
    }
    [ClientRpc]
    void UpdateAllButtonPresses()
    {
        transform.parent.GetComponent<ColourMiniGameManger>().sendPressedColour(colour, mat);
        print("We syncing the button y'all");
        transform.GetChild(0).GetComponent<Animator>().Play("Click");
    }
}




