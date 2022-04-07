using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("Server Started!");
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        Debug.Log("Server Stopped!");
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        Debug.Log("Connected to Server!");
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        Debug.Log("Disconnected from Server!");
        playerExitsOrDisconnects(conn);
    }

    void playerExitsOrDisconnects(NetworkConnection conn)
    {
        /*
        //Remove all objects owned by the player
        var ownedObjects = new NetworkIdentity[conn.clientOwnedObjects.Count];
        conn.clientOwnedObjects.CopyTo(ownedObjects);
        foreach (var networkIdentity in ownedObjects)
        {
            networkIdentity.RemoveClientAuthority();
        }
        */
        //Delete Player
        NetworkServer.Destroy(conn.identity.gameObject);
        if (NetworkServer.connections.Count == 0)
        {
            this.ServerChangeScene("Lobby");
        }
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
        print("Scene changed to " + sceneName);
    }
}
