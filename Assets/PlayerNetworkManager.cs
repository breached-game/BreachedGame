using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerNetworkManager : NetworkBehaviour
{
    private NetworkIdentity networkIdentity;
    // Pass in the gameobject, data, 
     void Awake()
    {
        networkIdentity = GetComponent<NetworkIdentity>();
    }

    #region 

    [Command]
    public void CmdMoveControlRod(Vector3 direction, float magnitude, GameObject controlRod)
    {
        Vector3 force = direction * magnitude;
        CmdUpdateControlRodMovement(force, controlRod);
    }

    [ClientRpc]
    public void CmdUpdateControlRodMovement(Vector3 force, GameObject controlRod)
    {
        controlRod.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }


    [Command]

    public void CmdSendCurrentPlayer(GameObject player, GameObject controlRodController)
    {
        SetCurrentPlayer(player, controlRodController);
    }

    [ClientRpc]
    public void SetCurrentPlayer(GameObject player, GameObject controlRodController)
    {
        controlRodController.gameObject.GetComponent<ControlRodTransport>().currentPlayer = player;
    }


    #endregion



}
