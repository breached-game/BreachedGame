using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    private NetworkConnection playerConn;
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
        Debug.Log("Connected to Server!");
        playerConn = conn;
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        Debug.Log("Disconnected from Server!");

        //Remove all objects owned by the player
        var ownedObjects = new NetworkIdentity[conn.clientOwnedObjects.Count];
        conn.clientOwnedObjects.CopyTo(ownedObjects);
        foreach (var networkIdentity in ownedObjects)
        {
            networkIdentity.RemoveClientAuthority();
        }
    }

    public override void OnApplicationQuit()
    {
       
        print("Here");
        //Remove all objects owned by the player
        var ownedObjects = new NetworkIdentity[playerConn.clientOwnedObjects.Count];
        playerConn.clientOwnedObjects.CopyTo(ownedObjects);
        print(ownedObjects.Length);
        foreach (var networkIdentity in ownedObjects)
        {
            networkIdentity.RemoveClientAuthority();
        }

        base.OnApplicationQuit();
    }
}
