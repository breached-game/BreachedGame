using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    public NetworkConnection currentNetworkConnection;
    public override void OnStartServer()
    {
        Debug.Log("Server Started!");
    }

    public override void OnStopServer()
    {
        Debug.Log("Server Stopped!");
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        currentNetworkConnection = conn;
        Debug.Log("Connected to Server!");
        GameObject.Find("MicManager").GetComponent<MicManagerScript>().OnConnection();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        Debug.Log("Disconnected from Server!");
    }

    public uint GetNetworkIdetity()
    {
        Debug.Log("Got Network Identiy");
        return currentNetworkConnection.identity.netId;
    }
}
