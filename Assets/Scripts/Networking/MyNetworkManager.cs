using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
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
}
