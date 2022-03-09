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

    private GameObject currentPlayer = null;

    private int movementStep = 1;

    public void EnteredController(GameObject player)
    {
        if (currentPlayer == null)
        {
            currentPlayer = player;
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

    }

    public void ExitController()
    {
        Cursor.lockState = CursorLockMode.Locked;
       
        if (currentPlayer.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            currentPlayer.GetComponent<PlayerManager>().FirstPersonCamera.SetActive(true);
            //TRASH CODING PRACTICE INBUILT SPEED
            currentPlayer.GetComponent<PlayerManager>().Speed = 4.0f;
            currentPlayer.GetComponent<PlayerManager>().disableInteractionsForMinigame = false;
            controlRodCamera.SetActive(false);
            controlRodUI.SetActive(false);
            playerUI.SetActive(false);

            currentPlayer = null;
               
        }
        
    }



    public void CallButtonPressed(Vector3Int direction) //Control rod button calls this script. This object has authortiy therefore can call command
    {
        CmdMoveControlRod(direction);
    }

    [Command]
    public void CmdMoveControlRod(Vector3Int direction)
    {
        Vector3 newPostion = controlRod.gameObject.transform.position + direction * movementStep;
        CmdUpdateControlRodMovememt(newPostion);
        

    }
    [ClientRpc]
    public void CmdUpdateControlRodMovememt(Vector3 newPos)
    {
        controlRod.gameObject.transform.position = newPos;
    }


   /* private void OnTriggerEnter(Collider other)
    {
        int numChildren;

        Cursor.lockState = CursorLockMode.None;
        other.gameObject.GetComponent<PlayerManager>().FirstPersonCamera.SetActive(false);
        //STOPPING PLAYER MOVING WHILE IN CONTROL ROD - MAYBE CHANGE PLAYERMANAGER TO HAVE A BOOL INSTEAD OF SETTING IT AS 0
        other.gameObject.GetComponent<PlayerManager>().Speed = 0.0f;
        controlRodCamera.SetActive(true);

        controlRodUI.SetActive(true);
        numChildren = controlRodUI.transform.childCount;
        for (int i = 0; i < numChildren; i++)
        {
            controlRodUI.transform.GetChild(i).gameObject.SetActive(true);
        }

        if (gameObject.name == "XMove")
        {
            controlRodUI.transform.GetChild(1).gameObject.SetActive(false);
            controlRodUI.transform.GetChild(2).gameObject.SetActive(false);
        } else if (gameObject.name == "YMove")
        {
            controlRodUI.transform.GetChild(0).gameObject.SetActive(false);
            controlRodUI.transform.GetChild(2).gameObject.SetActive(false);
        } else if (gameObject.name == "ZMove")
        {
            controlRodUI.transform.GetChild(0).gameObject.SetActive(false);
            controlRodUI.transform.GetChild(1).gameObject.SetActive(false);
        }
    }*/

}
