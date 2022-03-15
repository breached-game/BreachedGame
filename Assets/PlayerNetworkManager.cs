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

    #region:ControlRod
    public void MoveControlRod(Vector3 direction, float magnitude, GameObject controlRod)
    {
        Vector3 force = direction * magnitude;
        CmdMoveControlRod(force, controlRod);
    }


    [Command]
    public void CmdMoveControlRod(Vector3 force, GameObject controlRod)
    {
        CmdUpdateControlRodMovement(force, controlRod);
    }

    [ClientRpc]
    public void CmdUpdateControlRodMovement(Vector3 force, GameObject controlRod)
    {
        controlRod.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }

    public void SendCurrentPlayer(GameObject player, GameObject controlRodController) 
    {
        CmdSendCurrentPlayer(player, controlRodController);
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

    #region:StartButton
    public void StartGame(GameObject lights, GameObject timer, GameObject playerUI, GameObject spawnPoint, int gameTime, GameObject[] items)
    {
        CmdStartGame(lights, timer, playerUI, spawnPoint, gameTime, items);
    }

    [Command]
    public void CmdStartGame(GameObject lights, GameObject timer, GameObject playerUI, GameObject spawnPoint, int gameTime, GameObject[] items)
    {
        NetworkServer.SpawnObjects();
        updateStartGame(lights, timer, playerUI, spawnPoint, gameTime, items);
    }

    [ClientRpc]
    void updateStartGame(GameObject lights, GameObject timer, GameObject playerUI, GameObject spawnPoint, int gameTime, GameObject[] items)
    {
        GameObject[] players;
        //Bad practice we should pass players in some other way 
        players = GameObject.FindGameObjectsWithTag("Player");
        lights.GetComponent<LightManager>().TurnPressureAlarmOn();
        timer.GetComponent<TimerManager>().startTimer(gameTime);
        foreach (GameObject player in players)
        {
            player.transform.position = spawnPoint.transform.position;
            playerUI.SetActive(true);
            //GARBAGE CODING PRACTICE BELOW
            int children = playerUI.transform.childCount;
            for (int i = 0; i < children; i++)
            {
                playerUI.transform.GetChild(i).gameObject.SetActive(true);
            }
            player.GetComponent<PlayerManager>().TurnOnAudio();
        }
        foreach (GameObject item in items)
        {
            item.SetActive(true);
        }
    }
    #endregion


}
