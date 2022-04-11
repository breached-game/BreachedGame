using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerNetworkManager : NetworkBehaviour
{
    [SyncVar]
    private float masterTime;
    [SyncVar]
    bool timerStarted = false;
    SyncListString ColourCombo = new SyncListString();

    float time = 300f;
    int increments = 2000;

    private bool starter = false;

    private GameObject Timer;
    private TimerManager timerManager;

    private GameObject networkManager;
    private MyNetworkManager myNetworkManager;

    private NetworkIdentity networkIdentity;

    private FirstPersonController cameraController;

    private GameObject alarms;
    private PressureAlarm alarmManager;

    string[] colours = new string[] { "red", "green", "blue" };
    private Setup setupManager;
    private int comboLength = 5;

    // Pass in the gameobject, data, 
    void Awake()
    {
        networkIdentity = GetComponent<NetworkIdentity>();
    }

    void Start()
    {
        networkManager = GameObject.Find("NetworkManager");
        myNetworkManager = networkManager.GetComponent<MyNetworkManager>();
        cameraController = gameObject.GetComponentInChildren<FirstPersonController>();
        ColourCombo.Callback += onComboUpdated;
    }

    #region:SceneChange
    [Command]
    public void CmdChangeScene(string scene)
    {
        myNetworkManager.ServerChangeScene(scene);
    }

    public void ChangeToSub()
    {
        float time = 15;
        starter = true;
        CmdChangeScene("Orientation");
        StartCoroutine(OrientationTime(time));
    }

    IEnumerator OrientationTime(float time)
    {
        yield return new WaitForSeconds(time);
        CmdChangeScene("Submarine");
    }
    #endregion

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

    #region:Periscope
    public void PeriscopeSendCurrentPlayer(GameObject player, GameObject controlRodController)
    {
        CmdPeriscopeSendCurrentPlayer(player, controlRodController);
    }

    [Command]

    public void CmdPeriscopeSendCurrentPlayer(GameObject player, GameObject controlRodController)
    {
        PeriscopeSetCurrentPlayer(player, controlRodController);
    }

    [ClientRpc]
    public void PeriscopeSetCurrentPlayer(GameObject player, GameObject controlRodController)
    {
        controlRodController.gameObject.GetComponent<PeriscopeView>().currentPlayer = player;
    }
    #endregion

    #region:Orientation
    public void StartOrientation()
    {
        CmdStartOrientation();
    }
    [Command]
    public void CmdStartOrientation()
    {
        NetworkServer.SpawnObjects();
    }
    #endregion

    #region:StartButton
    public void StartGame(GameObject setupObject)
    {
        if (starter)
        {
            CmdStartGame(setupObject);
            starter = false;
            CmdAssignSkin();
        }
    }

    [Command]
    public void CmdStartGame(GameObject setupObject)
    {
        NetworkServer.SpawnObjects();
        CallUpdateStartGame(setupObject);
        StartCoroutine(masterTimer());
        timerStarted = true;
        StartCoroutine(AlarmTimer());
        SetColourCombo();
    }

    private void SetColourCombo()
    {
        int r;
        for (int i = 0; i < comboLength; i++)
        {
            r = Random.Range(0, colours.Length - 1);
            ColourCombo.Add(colours[r]);
        }
    }

    [ClientRpc]
    void CallUpdateStartGame(GameObject setupObject)
    {
        //setupObject.GetComponent<StartGameButton>().UpdateStartGame();
        setupManager = setupObject.GetComponent<Setup>();
        Timer = setupObject.GetComponent<Setup>().timer;
        timerManager = Timer.GetComponent<TimerManager>();
        alarmManager = setupObject.GetComponent<Setup>().alarms.GetComponent<PressureAlarm>();
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
        pressureAlarm.GetComponent<PressureAlarm>().StopAlarm();
    }
    #endregion

    #region:PressureAlarm
    IEnumerator AlarmTimer()
    {
        float wait = 0f;
        float duration = 0f;
        while (true)
        {
            wait = Random.Range(20, 30);
            yield return new WaitForSeconds(wait);
            duration = Random.Range(3, 7);
            TurnAlarmOn();
            yield return new WaitForSeconds(1);
            ShakeCameraOn();
            yield return new WaitForSeconds(duration);
            ShakeCameraOff();
            yield return new WaitForSeconds(1);
            TurnAlarmOff();
        }
    }

    [ClientRpc]
    void TurnAlarmOn()
    {
        alarmManager.StartAlarm();
    }

    [ClientRpc]
    void TurnAlarmOff()
    {
        alarmManager.StopAlarm();
    }

    [ClientRpc]
    void ShakeCameraOn()
    {
        cameraController.StartShake();
    }

    [ClientRpc]
    void ShakeCameraOff()
    {
        cameraController.StopShake();
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
    public void CallRemoveWaterPump(GameObject waterPump)
    {
        CmdRemoveWaterPump(waterPump);
    }

    [Command]
    public void CmdRemoveWaterPump(GameObject waterPump)
    {
        CallRemoveAllWaterPump(waterPump);
    }

    [ClientRpc]
    public void CallRemoveAllWaterPump(GameObject waterPump)
    {
        waterPump.GetComponent<WaterManager>().RemovePump();
    }

    #endregion

    #region Breach
    public void StopBreach(GameObject breach)
    {
        CmdStopBreach(breach);
    }

    [Command]
    public void CmdStopBreach(GameObject breach)
    {
        StopAllBreaches(breach);
    }

    [ClientRpc]
    public void StopAllBreaches(GameObject breach)
    {
        breach.GetComponent<WaterManager>().StopBreach();
    }

    #endregion

    #region:ColourButtons

    void onComboUpdated(SyncListString.Operation op, int index, string oldColour, string newColour)
    {
        switch (op)
        {
            case SyncListString.Operation.OP_ADD:
                // index is where it was added into the list
                // newItem is the new item
                if (!isServer)
                {
                    if (ColourCombo.Count == comboLength)
                    {
                        List<string> correctColourCombo = new List<string>();
                        foreach (var colour in ColourCombo)
                        {
                            correctColourCombo.Add(colour);
                        }
                        setupManager.SetColourCombo(correctColourCombo);
                    }
                }
                break;
            case SyncListString.Operation.OP_INSERT:
                // index is where it was inserted into the list
                // newItem is the new item
                break;
            case SyncListString.Operation.OP_REMOVEAT:
                // index is where it was removed from the list
                // oldItem is the item that was removed
                break;
            case SyncListString.Operation.OP_SET:
                // index is of the item that was changed
                // oldItem is the previous value for the item at the index
                // newItem is the new value for the item at the index
                break;
            case SyncListString.Operation.OP_CLEAR:
                // list got cleared
                break;
        }
    }

    public void ButtonPressed(GameObject button)
    {
        CmdButtonPressed(button);
    }

    [Command]
    public void CmdButtonPressed(GameObject button)
    {
        CallUpdateAllButtonPresses(button);
        //button.transform.parent.GetComponent<ColourMiniGameManger>().sendPressedColour(colour, mat);
        button.transform.GetChild(0).GetComponent<Animator>().Play("Click");
    }

    [ClientRpc]
    public void CallUpdateAllButtonPresses(GameObject button)
    {
        button.GetComponent<ColourMiniGameButton>().UpdateAllButtonPresses();
    }
    #endregion

    #region:Timer
    IEnumerator masterTimer()
    {
        masterTime = 0f;
        while (masterTime < time)
        {
            UpdateTime();
            masterTime += (time / increments);
            yield return new WaitForSeconds(time / increments);
        }
    }

    [ClientRpc]
    private void UpdateTime()
    {
        if (timerStarted)
        {
            timerManager.UpdateTimer(masterTime, time, increments);
        }
    }
    #endregion

    #region:ServerDoors

    public void OnLeverUp(GameObject lever)
    {
        CmdOnLeverUp(lever);
    }
    [Command]
    public void CmdOnLeverUp(GameObject lever)
    {
        CallUpdateAllOnLeverUp(lever);
    }
    [ClientRpc]
    public void CallUpdateAllOnLeverUp(GameObject lever)
    {
        lever.GetComponent<ServerRoomDoorSwitch>().UpdateOnLeverChange();
    }

    public void ServerButtonPressed(GameObject button)
    {
        CmdServerButtonPressed(button);
    }

    [Command]
    public void CmdServerButtonPressed(GameObject button)
    {
        CallUpdateServerButtonPressed(button);
        //button.transform.parent.GetComponent<ColourMiniGameManger>().sendPressedColour(colour, mat);
        button.transform.GetChild(0).GetComponent<Animator>().Play("Click");
    }

    [ClientRpc]
    public void CallUpdateServerButtonPressed(GameObject button)
    {
        button.GetComponent<ResetServerButton>().ResetButtonPressed();
    }



    #endregion

    #region Doors
    public void OpenDoor(GameObject door)
    {
        CmdOpenDoor(door);
    }
    [Command]
    public void CmdOpenDoor(GameObject door)
    {
        CallOpenDoor(door);
    }
    [ClientRpc]
    public void CallOpenDoor(GameObject door)
    {
        door.GetComponent<Door>().OpenDoor();
    }
    #endregion

    #region:CommandLine

    public void SpawnCommandLine(GameObject commandNetwork)
    {
        CmdSpawnCommandLine(commandNetwork);
    }
    [Command]
    public void CmdSpawnCommandLine(GameObject commandNetwork)
    {
        if (!commandNetwork.GetComponent<NetworkIdentity>().isActiveAndEnabled)
        {
            NetworkServer.Spawn(commandNetwork);
        }
        NetworkServer.Spawn(commandNetwork);
    }
    public void WriteCommand(GameObject commandNetwork, string msg, bool captain = false)
    {
        CmdWriteCommand(commandNetwork, msg, captain);
    }
    [Command]
    public void CmdWriteCommand(GameObject commandNetwork, string msg, bool captain)
    {
        print("command");
        CallNetworkQueueMessage(commandNetwork, msg, captain);
    }
    [ClientRpc]
    public void CallNetworkQueueMessage(GameObject commandNetwork,string msg, bool captain)
    {
        commandNetwork.GetComponent<CommandNetworkManager>().QueueNetworkMessage(msg, captain);
        print("message");
    }
    #endregion

    #region:Assign Players Skins and Names
    public List<Material> playerMats;
    private Dictionary<GameObject, string> playerNames;
    [Command]
    public void CmdAssignSkin()
    {
        //Bad practice we should pass players in some other way 
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        int m = 0;
        foreach (GameObject player in players)
        {
            SetNameOnServer(player, m);
            m++;
        }
    }

    [ClientRpc]
    public void SetNameOnServer(GameObject player, int m)
    {
        if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            CmdSetNameOnServer(player, PlayerPrefs.GetString("Name"), m);
        }
    }

    [Command]
    public void CmdSetNameOnServer(GameObject player, string name, int m)
    {
        print("Player: " + name);
        CallUpdateSetName(player, name, m);
    }

    [ClientRpc]
    public void CallUpdateSetName(GameObject player, string name, int m)
    {
        print("Player: " + name);
        player.GetComponent<NameTagManager>().SetName(name);
        player.GetComponent<PlayerManager>().PlayerModel.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = playerMats[m];
    }
    #endregion
}
