using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ColourMiniGameButton : NetworkBehaviour
{
    public string colour;
    [Command]
    public void buttonPressed()
    {
        print("Called Button Pressed");
        syncButtonPress();
    }
    [ClientRpc]
    void syncButtonPress()
    {
        print("Called");
        transform.parent.GetComponent<ColourMiniGameManger>().sendPressedColour(colour);
    }
}

