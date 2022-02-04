using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class LobbyUIManager : MonoBehaviour
{
    public Text NumberOfPlayers;

    // Update is called once per frame
    void Update()
    {
        // HARD coded stuff //

        NetworkManager nm = FindObjectOfType<NetworkManager>();
        //TO DO Find out how to get from PhotonNetwork to max players per room, it just annoys me that this is hard coded :(
        NumberOfPlayers.text = "" + nm.numPlayers + "/2";
        if (nm.numPlayers < 2)
        {
            //StartButton.interactable = false;
        }
        else if (nm.numPlayers == 2)
        {
            //StartButton.interactable = true;
        }
        else print("Programmed max players (hard coded) so this can cause this error");

    }
}
