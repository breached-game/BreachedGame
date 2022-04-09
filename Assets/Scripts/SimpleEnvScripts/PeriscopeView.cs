using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PeriscopeView : NetworkBehaviour
{
    [SyncVar]
    public GameObject currentPlayer = null;
    private PlayerManager playerManager;
    private PlayerNetworkManager playerNetworkManager;
    public GameObject pericopeView;
    public GameObject playerUI;
    public GameObject pericopeUI;
    public void EnteredPeriscope(GameObject player)
    {
        if (currentPlayer == null)
        {
            if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                playerManager = player.GetComponent<PlayerManager>();
                playerNetworkManager = player.GetComponent<PlayerNetworkManager>();
                playerNetworkManager.PeriscopeSendCurrentPlayer(player, this.gameObject);

                Cursor.lockState = CursorLockMode.None;
                playerManager.FirstPersonCamera.SetActive(false);
                //STOPPING PLAYER MOVING WHILE IN CONTROL ROD - MAYBE CHANGE PLAYERMANAGER TO HAVE A BOOL INSTEAD OF SETTING IT AS 0
                playerManager.Speed = 0.0f;
                pericopeView.SetActive(true);
                pericopeUI.SetActive(true);
                playerUI.SetActive(false);
            }
        }
        else
        {
            print("Controller occupied by another player");
        }

    }
    public void ExitPeriscope()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (!isServer)
        {
            if (currentPlayer.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                playerManager.FirstPersonCamera.SetActive(true);
                //TRASH CODING PRACTICE INBUILT SPEED
                playerManager.Speed = playerManager.defaultSpeed;
                playerManager.disableInteractionsForMinigame = false;
                pericopeView.SetActive(false);
                pericopeUI.SetActive(false);
                playerUI.SetActive(true);

                playerNetworkManager.PeriscopeSendCurrentPlayer(null, this.gameObject);
            }
        }

    }
}
