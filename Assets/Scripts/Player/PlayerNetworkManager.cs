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
    }

    public void ChangeToSub()
    {
        starter = true;
        CmdChangeScene("Submarine");
    }

    #region:SceneChange
    [Command]
    public void CmdChangeScene(string scene)
    {
        myNetworkManager.ServerChangeScene(scene);
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

    #region:StartButton
    public void StartGame(GameObject setupObject)
    {
        if (starter)
        {
            CmdStartGame(setupObject);
            starter = false;
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
    }

    [ClientRpc]
    void CallUpdateStartGame(GameObject setupObject)
    {
        //setupObject.GetComponent<StartGameButton>().UpdateStartGame();
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

    #region:Timer
    IEnumerator masterTimer()
    {
        masterTime = 0f;
        while (masterTime < time) {
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

    #endregion

    #region:Assign Players Skins and Names
    public List<Material> playerMats;
    public void AssignSkin()
    {
        CmdAssignSkin();
    }
    [Command]
    public void CmdAssignSkin()
    {
        CallUpdateAssignSkin();
    }
    [ClientRpc]
    public void CallUpdateAssignSkin()
    {
        //Bad practice we should pass players in some other way 
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        int i = 0;
        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkIdentity>().isLocalPlayer) CmdSetName(player, PlayerPrefs.GetString("Name"));
            player.GetComponent<PlayerManager>().PlayerModel.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = playerMats[i];
            i++;
        }
    }
    [Command]
    public void CmdSetName(GameObject player, string name)
    {
        CallUpdateSetName(player, name);
    }
    [ClientRpc]
    public void CallUpdateSetName(GameObject player, string name)
    {
        player.GetComponent<NameTagManager>().SetName(name);
    }
    #endregion
}
