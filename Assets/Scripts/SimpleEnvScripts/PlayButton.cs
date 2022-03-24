using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayButton : MonoBehaviour
{
    public Material MouseOnColour;
    private Material preMat;
    public GameObject networkManager;
    public GameObject nameManager;
    private MyNetworkManager myNetworkManager;

    private void Start()
    {
        preMat = GetComponent<MeshRenderer>().material;
        if (!(networkManager == null))
        {
            myNetworkManager = networkManager.GetComponent<MyNetworkManager>();
        }
        else { print("no network manager"); }
    }
    private void OnMouseEnter()
    {
            GetComponent<MeshRenderer>().material = MouseOnColour;
    }
    private void OnMouseExit()
    {
            GetComponent<MeshRenderer>().material = preMat;
    }
    private void OnMouseDown()
    {
        //myNetworkManager.ServerChangeScene("Lobby");
        if (!NetworkClient.active)
        {
            PlayerPrefs.SetString("Name", nameManager.GetComponent<TypeInNameMainMenui>().playerName);
            myNetworkManager.StartClient();
        }
        else print("Trying to connect when already connected");
    }
}
