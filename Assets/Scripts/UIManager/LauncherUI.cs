using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class LauncherUI : MonoBehaviour
{
    private NetworkManager manager;
    public GameObject status;
    public GameObject networkManager;

    public GameObject LobbyUI;
    public GameObject PlayerUI;

    public GameObject MenuCamera;
    public GameObject crosshair;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        StatusLabels();

        //Start player disable UI
        if (NetworkClient.isConnected)
        {
            if (ClientScene.localPlayer == null)
            {
                ClientScene.AddPlayer(NetworkClient.connection);
                LobbyUI.SetActive(true);
                PlayerUI.SetActive(true);

                //TRASH CODING PRACTICE BELOW
                int children = PlayerUI.transform.childCount;
                for (int i = 0; i < children; i++)
                {
                    PlayerUI.transform.GetChild(i).gameObject.SetActive(false);
                }
                crosshair.SetActive(true);
            }
        }

        if (LobbyUI.activeSelf)
        {
            MenuCamera.SetActive(false);
            LobbyUI.GetComponent<Text>().text = countObjects() + "/" + GameObject.Find("NetworkManager").GetComponent<MyNetworkManager>().maxConnections;
        }
    }

    int countObjects()
    {

        var go = GameObject.FindGameObjectsWithTag("Player");
        int counter = 0;

        for (var i = 0; i < go.Length; i++)
        {
            counter++;
        }

        return counter;
    }

    public void StartButtonHostClient()
    {
        if (!NetworkClient.active)
        {
            manager.StartHost();
        }
        else print("Trying to connect when already connected");
    }

    public void StartButtonClient()
    {
        if (!NetworkClient.active)
        {
            manager.StartClient();
        }
        else print("Trying to connect when already connected");
    }
    public void StartButtonServer()
    {
        if (!NetworkClient.active)
        {
            manager.StartServer();
        }
        else print("Trying to connect when already connected");
    }


    void StatusLabels()
    {
        // host mode
        // display separately because this always confused people:
        //   Server: ...
        //   Client: ...
        if (NetworkServer.active && NetworkClient.active)
        {
            status.GetComponent<Text>().text = "Host: running via " + Transport.activeTransport;
        }
        // server only
        else if (NetworkServer.active)
        {
            status.GetComponent<Text>().text = "Server: running via " + Transport.activeTransport;
        }
        // client only
        else if (NetworkClient.isConnected)
        {
            status.GetComponent<Text>().text = "Client: running via " + Transport.activeTransport + " via " + Transport.activeTransport;
        }
    }

    public void StopButtons()
    {
        // stop host if host mode
        if (NetworkServer.active && NetworkClient.isConnected)
        {
                manager.StopHost();
        }
        // stop client if client-only
        else if (NetworkClient.isConnected)
        {
                manager.StopClient();
        }
        // stop server if server-only
        else if (NetworkServer.active)
        {
                manager.StopServer();
        }
        Application.Quit();
    }
}
