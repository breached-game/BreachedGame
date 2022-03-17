using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayButton : MonoBehaviour
{
    public Material MouseOnColour;
    private Material preMat;
    public GameObject networkManager;
    private MyNetworkManager myNetworkManager;

    private void Start()
    {
        preMat = GetComponent<MeshRenderer>().material;
        if (!(networkManager == null))
        {
            myNetworkManager = networkManager.GetComponent<MyNetworkManager>();
        }
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
        myNetworkManager.ServerChangeScene("Lobby");
        if (!NetworkClient.active)
        {
            myNetworkManager.StartClient();
        }
        else print("Trying to connect when already connected");
    }
}
