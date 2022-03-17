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

    public GameObject playerPos;
    private Vector3 prePlayerPos;

    private PlayerManager playerManager;
    private PlayerNetworkManager playerNetworkManager;

    [SyncVar]
    public GameObject currentPlayer = null;

    public float magnitude;

    public void EnteredController(GameObject player)
    {
        if (currentPlayer == null)
        {
            if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                playerManager = player.GetComponent<PlayerManager>();
                playerNetworkManager = player.GetComponent<PlayerNetworkManager>();
                playerNetworkManager.SendCurrentPlayer(player, this.gameObject);

                Cursor.lockState = CursorLockMode.None;
                playerManager.FirstPersonCamera.SetActive(false);
                VisualEffectOfBeingInTheMiniGame(player);
                //STOPPING PLAYER MOVING WHILE IN CONTROL ROD - MAYBE CHANGE PLAYERMANAGER TO HAVE A BOOL INSTEAD OF SETTING IT AS 0
                playerManager.Speed = 0.0f;
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
        if (!isServer)
        {
            if (currentPlayer.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                playerManager.FirstPersonCamera.SetActive(true);
                VisualEffectOfLeavingAMiniGame(currentPlayer);
                //TRASH CODING PRACTICE INBUILT SPEED
                playerManager.Speed = 4.0f;
                playerManager.disableInteractionsForMinigame = false;
                controlRodCamera.SetActive(false);
                controlRodUI.SetActive(false);
                playerUI.SetActive(false);

                playerNetworkManager.SendCurrentPlayer(null, this.gameObject);
            }
        }
        
    }



    public void CallButtonPressed(Vector3 direction) //Control rod button calls this script. This object has authortiy therefore can call command
    {
        playerNetworkManager.MoveControlRod(direction, magnitude, controlRod);
    }

  
    //Visual effects
    public void VisualEffectOfBeingInTheMiniGame(GameObject player)
    {
        GameObject playerModel = player.GetComponent<PlayerManager>().PlayerModel;
        playerModel.GetComponent<Animator>().Play("SitDown");
        prePlayerPos = playerModel.transform.position;
        playerModel.transform.position = playerPos.transform.position;
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    public void VisualEffectOfLeavingAMiniGame(GameObject player)
    {
        GameObject playerModel = player.GetComponent<PlayerManager>().PlayerModel;
        playerModel.GetComponent<Animator>().Play("Idle");
        playerModel.transform.position = prePlayerPos;
    }
}