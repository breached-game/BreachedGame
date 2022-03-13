using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ControlRodTransport : NetworkBehaviour
{
    public GameObject controlRodCamera;
    public GameObject controlRodUI;
    public GameObject controlRod;
    public GameObject playerUI;

    [SyncVar]
    private GameObject currentPlayer = null;

    public float magnitude;

    public void EnteredController(GameObject player)
    {
        if (currentPlayer == null)
        {
           currentPlayer = player;
           CmdSendCurrentPlayer(player);
            if (currentPlayer.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                Cursor.lockState = CursorLockMode.None;
                player.gameObject.GetComponent<PlayerManager>().FirstPersonCamera.SetActive(false);
                //STOPPING PLAYER MOVING WHILE IN CONTROL ROD - MAYBE CHANGE PLAYERMANAGER TO HAVE A BOOL INSTEAD OF SETTING IT AS 0
                player.gameObject.GetComponent<PlayerManager>().Speed = 0.0f;
                controlRodCamera.SetActive(true);
                controlRodUI.SetActive(true);
            }
        }
        else
        {
            print("Controller occupied by another player");
        }

    }

    public void ExitController()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (!isServer || currentPlayer == null)
        {
            if (currentPlayer.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                currentPlayer.GetComponent<PlayerManager>().FirstPersonCamera.SetActive(true);
                //TRASH CODING PRACTICE INBUILT SPEED
                currentPlayer.GetComponent<PlayerManager>().Speed = 4.0f;
                currentPlayer.GetComponent<PlayerManager>().disableInteractionsForMinigame = false;
                controlRodCamera.SetActive(false);
                controlRodUI.SetActive(false);
                playerUI.SetActive(false);

                CmdSendCurrentPlayer(null);
            }
        }
        
    }



    public void CallButtonPressed(Vector3 direction) //Control rod button calls this script. This object has authortiy therefore can call command
    {
        CmdMoveControlRod(direction);
    }

    [Command] 
    public void CmdSendCurrentPlayer(GameObject player)
    {
        currentPlayer = player;
    }
    /*[ClientRpc]
    public void CmdUpdateCurrentPlayer(GameObject player)
    {
        currentPlayer = player;
    }*/


    [Command]
    public void CmdMoveControlRod(Vector3 direction)
    {
        Vector3 force = direction * magnitude;
        CmdUpdateControlRodMovement(force);
    }

    [ClientRpc]
    public void CmdUpdateControlRodMovement(Vector3 force)
    {
        controlRod.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }
}
