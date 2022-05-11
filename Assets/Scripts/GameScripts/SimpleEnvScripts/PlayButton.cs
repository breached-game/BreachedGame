using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayButton : MonoBehaviour
{
    /*
        SCRIPT FOR THE PLAY BUTTON IN THE MAIN MENU

        Contributors: Andrew Morgan
    */
    public Material MouseOnColour;
    private Material preMat;
    public GameObject networkManager;
    public GameObject nameManager;
    private MyNetworkManager myNetworkManager;

    private void Start()
    {
        // Checks for a network manager as that is essential to play the game
        preMat = GetComponent<MeshRenderer>().material;
        if (!(networkManager == null))
        {
            myNetworkManager = networkManager.GetComponent<MyNetworkManager>();
        }
        else { print("no network manager"); }
    }

    // Change colour of play button depending on whether the mouse is hovering over it
    private void OnMouseEnter()
    {
        GetComponent<MeshRenderer>().material = MouseOnColour;
    }
    private void OnMouseExit()
    {
        GetComponent<MeshRenderer>().material = preMat;
    }

    // Starts the network client when the play button is pressed
    private void OnMouseDown()
    {
        // Locks cursor for entering the game
        Cursor.lockState = CursorLockMode.Locked;
        // Avoids multiple clients being created from the same machine
        if (!NetworkClient.active)
        {
            PlayerPrefs.SetString("Name", nameManager.GetComponent<TypeInNameMainMenui>().playerName);
            myNetworkManager.StartClient();
        }
        else print("Trying to connect when already connected");
    }
}
