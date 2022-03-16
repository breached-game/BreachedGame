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
    public void StartGame(GameObject startButton)
    {
        CmdStartGame(startButton);
    }

    [Command]
    public void CmdStartGame(GameObject startButton)
    {
        NetworkServer.SpawnObjects();
        CallUpdateStartGame(startButton);
    }

    [ClientRpc]
    void CallUpdateStartGame(GameObject startButton)
    {
        startButton.GetComponent<StartGameButton>().UpdateStartGame();
    }
    #endregion

    #region:PressureButton
    public void PressureAlarmPress(GameObject pressureAlarm)
    {
        CmdPressureAlarmPress(pressureAlarm);
    }
    [Command]
    public void CmdPressureAlarmPress(GameObject pressureAlarm)
    {
        CallPressureAlarmPress(pressureAlarm);
    }
    [ClientRpc]
    void CallPressureAlarmPress(GameObject pressureAlarm)
    {
        pressureAlarm.GetComponent<PressureAlarm>().PressureAlarmPress();
    }
    #endregion

    #region:DropOff
    public void DropOff(GameObject interactable)
    {
        Debug.Log("dropping off");
        CmdDropOff(interactable);
    }
    [Command]
    public void CmdDropOff(GameObject interactable)
    {
        CallDroppingOffItem(interactable);
    }

    [ClientRpc]
    public void CallDroppingOffItem(GameObject interactable)
    {
        interactable.GetComponent<DropOff>().DroppingOffItem(this.gameObject);
    }
    #endregion

    #region:Water
    public void OutflowWater(GameObject waterPump)
    {
        CmdOutflowWater(waterPump);
    }
    [Command]
    public void CmdOutflowWater(GameObject waterPump)
    {
        CallOutflowWater(waterPump);
    }

    [ClientRpc]
    public void CallOutflowWater(GameObject waterPump)
    {
        waterPump.GetComponent<WaterManager>().OutflowWater();
    }
    #endregion

    #region:ColourButtons
    public void ButtonPressed(GameObject button)
    {
        CmdButtonPressed(button);
    }

    [Command]
    public void CmdButtonPressed(GameObject button)
    {
        CallUpdateAllButtonPresses(button);
        //button.transform.parent.GetComponent<ColourMiniGameManger>().sendPressedColour(colour, mat);
        print("We syncing the button y'all");
        button.transform.GetChild(0).GetComponent<Animator>().Play("Click");
    }

    [ClientRpc]
    public void CallUpdateAllButtonPresses(GameObject button)
    {
        button.GetComponent<ColourMiniGameButton>().UpdateAllButtonPresses();
    }
    #endregion


}
