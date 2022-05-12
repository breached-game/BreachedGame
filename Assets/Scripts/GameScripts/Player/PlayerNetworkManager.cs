using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerNetworkManager : NetworkBehaviour
{
    /*
        STUBS SCRIPT FOR ALL NETWORKED FUNCTION CALLS FROM EACH PLAYER
        PLAYER MODEL IS ONLY OBJECT WITH AUTHORITY SO EVERYTHING MUST
        COME THROUGH THIS.

        GENERAL PATTERN IS:
        CLIENT CALLS FUNCTION IN THIS SCRIPT
        THAT FUNCTION CALLS [COMMAND] WHICH RUNS ON THE SERVER
        [COMMAND] CALLS [CLIENTRPC] WHICH RUNS ON EACH CLIENT

        Contributors: Sam Barnes-Thornton, Andrew Morgan, Seth Holdcroft, Srdjan Vojnovic, Luke Benson
    */

    // Sync variables for the master timer
    [SyncVar]
    private float masterTime;
    [SyncVar]
    bool timerStarted = false;
    // SyncList so that the random colour combination can be relayed to each client
    SyncListString ColourCombo = new SyncListString();

    // Length of time for main game to take
    float time = 480f;
    // Increments for time updates
    int increments = 2000;

    // Indicates whether particular player pressed the start game button
    private bool starter = false;

    private GameObject Timer;
    private TimerManager timerManager;
    private GameObject missileManager;

    private GameObject networkManager;
    private MyNetworkManager myNetworkManager;

    private NetworkIdentity networkIdentity;

    private FirstPersonController cameraController;

    private GameObject alarms;
    private PressureAlarm alarmManager;

    // Colour options for random combination
    string[] colours = new string[] { "red", "green", "blue" };
    private Setup setupManager;
    // Length of random colour combination
    private int comboLength = 5;
    private bool missileStarted = false;
    private bool gameEnded = false;

    private bool orientationEnded = false;

    // Possible names for players in the game
    string[] names = new string[] { "Lt.Barnes", "Lt.Holdcroft", "Lt.Morgan", "Lt.Vojnovic" };

    // Public list of materials for player models (each player is a different colour)
    public List<Material> playerMats;

    void Awake()
    {
        networkIdentity = GetComponent<NetworkIdentity>();
    }

    void Start()
    {
        // Sets various Unity object references for use later on
        networkManager = GameObject.Find("NetworkManager");
        myNetworkManager = networkManager.GetComponent<MyNetworkManager>();
        cameraController = gameObject.GetComponentInChildren<FirstPersonController>();
        // Sets callback for colour combo SyncList
        ColourCombo.Callback += onComboUpdated;
    }

    // Networking for scene changes
    #region:SceneChange

    // Changes the scene on the server
    [Command]
    public void CmdChangeScene(string scene)
    {
        myNetworkManager.ServerChangeScene(scene);
    }

    // Manages changing to the victory scene
    public void ChangeToVictory()
    {
        // Makes sure that multiple players don't try to change the scene
        if (!gameEnded)
        {
            gameEnded = true;
            CmdEndGame();
            // Turns off muffle
            if (GetComponent<PlayerManager>().inWater && Application.platform == RuntimePlatform.WebGLPlayer)
            {
                VoiceWrapper.waterMicOff();
            }
            // Turns off player cameras as they are not needed in victory scene
            CmdChangeCamera(false);
            CmdChangeScene("EndGameWin");
        }
    }

    // Manages changing to the lose scene
    public void ChangeToLose()
    {
        // Makes sure that multiple players don't try to change the scene
        if (!gameEnded)
        {
            gameEnded = true;
            CmdEndGame();
            // Turns off muffle
            if (GetComponent<PlayerManager>().inWater && Application.platform == RuntimePlatform.WebGLPlayer)
            {
                VoiceWrapper.waterMicOff();
            }
            // Turns off player cameras as they are not needed in victory scene
            CmdChangeCamera(false);
            CmdChangeScene("EndGameLose");
        }
    }

    // Changes from lobby to orientation
    public void ChangeToSub()
    {
        CmdSetPlayerNames();
        // Master player set as one that presses start button
        starter = true;
        CmdChangeScene("Orientation");
    }

    // Runs cut scene after dinner plate has been pressed
    IEnumerator FinishOrientationTime()
    {
        // Player cameras turned off as not needed
        CallChangeCameras(false);
        myNetworkManager.ServerChangeScene("StartGame");
        yield return new WaitForSeconds(7f);
        CallChangeCameras(true);
        myNetworkManager.ServerChangeScene("Submarine");
    }

    // Command to set all players' cameras to either be on or off
    [Command]
    public void CmdChangeCamera(bool active)
    {
        CallChangeCameras(active);
    }

    // RPC call setting cameras to be on or off
    [ClientRpc]
    public void CallChangeCameras(bool active)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                player.GetComponent<PlayerManager>().FirstPersonCamera.SetActive(active);
            }
        }
    }
    #endregion

    // Networking for the control rod minigame
    #region:ControlRod

    // Calculates force for movement and calls command
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

    // Relays the control rod movement to all clients
    [ClientRpc]
    public void CmdUpdateControlRodMovement(Vector3 force, GameObject controlRod)
    {
        // Moves control rod by adding a force to the rigid body
        controlRod.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }

    // Sends a command to let the server know which player is in a specific controller
    public void SendCurrentPlayer(GameObject player, GameObject controlRodController, GameObject lazyNetworkVisualSolution)
    {
        CmdSendCurrentPlayer(player, controlRodController, lazyNetworkVisualSolution);
    }

    [Command]

    public void CmdSendCurrentPlayer(GameObject player, GameObject controlRodController, GameObject lazyNetworkVisualSolution)
    {
        SetCurrentPlayer(player, controlRodController, lazyNetworkVisualSolution);
    }

    // Relays to all clients that a player has entered a controller
    [ClientRpc]
    public void SetCurrentPlayer(GameObject player, GameObject controlRodController, GameObject lazyNetworkVisualSolution)
    {
        controlRodController.gameObject.GetComponent<ControlRodTransport>().currentPlayer = player;

        //Visual Effect
        //Leaving

        if (player == null)
        {
            GameObject playerModel = lazyNetworkVisualSolution.GetComponent<PlayerManager>().PlayerModel;
            playerModel.GetComponent<Animator>().Play("Idle");
            playerModel.transform.localPosition = new Vector3(0, -1.6f, -0.4f);
        }
        //Entering
        else
        {
            GameObject playerModel = player.GetComponent<PlayerManager>().PlayerModel;
            playerModel.GetComponent<Animator>().Play("SitDown");
            GameObject PlayerPos = controlRodController.GetComponent<ControlRodTransport>().playerPos;
            playerModel.transform.position = PlayerPos.transform.position;
            controlRodController.GetComponent<ControlRodTransport>().prePlayerPos = playerModel.transform.position;
            player.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

    }
    #endregion

    // Networking for the start of the orientation scene
    #region:Orientation
    public void StartOrientation()
    {
        CmdStartOrientation();
    }

    // Simply spawns all of the network objects required for the orientation scene
    [Command]
    public void CmdStartOrientation()
    {
        NetworkServer.SpawnObjects();
    }
    #endregion

    // Networking for the start of the main game
    #region:StartButton

    // Called at the beinning of the submarine scene
    public void StartGame(GameObject setupObject)
    {
        // Only executes anything if the player is the one who pressed the lobby button
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
        StartCoroutine(WaitStartGame(setupObject));
    }

    // Coroutine used to wait for all players to be loaded into the scene before the timer is started
    IEnumerator WaitStartGame(GameObject setupObject)
    {
        yield return new WaitForSeconds(10);
        UpdateStartGame(setupObject);
        StartCoroutine(masterTimer());
        timerStarted = true;
        StartCoroutine(AlarmTimer());
        SetColourCombo();
    }

    // Assigns a name and skin to all players once they have moved into the submarine
    [Command]
    private void CmdSetPlayerNames()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        int skin = 0;
        foreach (GameObject player in players)
        {
            CallSetNameOnClient(player, skin);
            skin++;
        }
    }

    // Relays names and skins to all clients
    [ClientRpc]
    public void CallSetNameOnClient(GameObject player, int skin)
    {
        string n = names[skin];
        print("Player: " + n);
        player.GetComponent<NameTagManager>().SetName(n);
        player.GetComponent<PlayerManager>().PlayerModel.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = playerMats[skin];
    }

    // Generates the random colour combination for the missile task
    private void SetColourCombo()
    {
        int r;
        for (int i = 0; i < comboLength; i++)
        {
            r = Random.Range(0, colours.Length);
            ColourCombo.Add(colours[r]);
        }
    }

    // Used to setup object references on clients for the submarine scene
    // Without these references the timer and alarms will fail
    [ClientRpc]
    public void UpdateStartGame(GameObject setupObject)
    {
        setupManager = setupObject.GetComponent<Setup>();
        Timer = setupObject.GetComponent<Setup>().timer;
        timerManager = Timer.GetComponent<TimerManager>();
        alarmManager = setupObject.GetComponent<Setup>().alarms.GetComponent<PressureAlarm>();
        missileManager = setupObject.GetComponent<Setup>().missileTimerText;
        timerStarted = true;
    }
    #endregion

    // Legacy code for a button which turns off the alarms
    // Left in to show development stages
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

    // Timing for the pressure alarms
    #region:PressureAlarm

    // Coroutine for initiating alarm sounds and shaking at random intervals
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

    // Checks for object reference and starts alarm
    [ClientRpc]
    void TurnAlarmOn()
    {
        if (alarmManager == null)
        {
            print("Alarm manager not set");
        }
        else
        {
            alarmManager.StartAlarm();
        }
    }

    // Checks for object reference and stops alarm
    [ClientRpc]
    void TurnAlarmOff()
    {
        if (alarmManager == null)
        {
            print("Alarm manager not set");
        }
        else
        {
            alarmManager.StopAlarm();
        }
    }

    // Relays camera shake to all clients
    [ClientRpc]
    void ShakeCameraOn()
    {
        cameraController.StartShake();
    }

    // Relays camera shake off to all clients
    [ClientRpc]
    void ShakeCameraOff()
    {
        cameraController.StopShake();
    }
    #endregion

    // Networking for dropping off items (now only batteries in the game)
    #region:DropOff

    // Called by a client to say an item has been dropped off
    public void DropOff(GameObject interactable)
    {
        CmdDropOff(interactable);
    }
    [Command]
    public void CmdDropOff(GameObject interactable)
    {
        CallDroppingOffItem(interactable);
    }

    // Relays to all clients that an item has been dropped off
    [ClientRpc]
    public void CallDroppingOffItem(GameObject interactable)
    {
        interactable.GetComponent<DropOff>().DroppingOffItem(this.gameObject);
    }
    #endregion

    // Networking for the interactions between the water pump and the water
    #region:Water

    // Called by a client to say they have removed the water pump
    public void CallRemoveWaterPump(GameObject waterPump)
    {
        CmdRemoveWaterPump(waterPump);
    }

    [Command]
    public void CmdRemoveWaterPump(GameObject waterPump)
    {
        CallRemoveAllWaterPump(waterPump);
    }

    // Relays to all clients that a water pump has been removed
    [ClientRpc]
    public void CallRemoveAllWaterPump(GameObject waterPump)
    {
        waterPump.GetComponent<WaterManager>().RemovePump();
    }

    #endregion

    // Networking for stopping and starting breaches
    #region Breach

    // Called by a client to stop a breach (when wood has been placed)
    public void StopBreach(GameObject breach)
    {
        CmdStopBreach(breach);
    }

    [Command]
    public void CmdStopBreach(GameObject breach)
    {
        StopAllBreaches(breach);
    }

    // Relays the call to stop a breach to all clients
    [ClientRpc]
    public void StopAllBreaches(GameObject breach)
    {
        breach.GetComponent<WaterManager>().StopBreach();
    }

    #endregion

    // Networking for the colour combination buttons for the missiles
    // Also includes the missile countdown timer
    #region:ColourButtons

    // Callback function for when the random combination is generated
    void onComboUpdated(SyncListString.Operation op, int index, string oldColour, string newColour)
    {
        switch (op)
        {
            // We are only interested in when a colour is added to the list
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

    // Called by a client when they press a button
    public void ButtonPressed(GameObject button)
    {
        CmdButtonPressed(button);
    }

    [Command]
    public void CmdButtonPressed(GameObject button)
    {
        CallUpdateAllButtonPresses(button);
        button.transform.GetChild(0).GetComponent<Animator>().Play("Click");
    }

    // Relays that a button has been pressed (will play animation on all clients
    [ClientRpc]
    public void CallUpdateAllButtonPresses(GameObject button)
    {
        button.GetComponent<ColourMiniGameButton>().UpdateAllButtonPresses();
    }

    // Called by a client to start the missile timer when all obectives have been completed
    public void StartMissileTimer()
    {
        CmdStartMissileTimer();
    }

    // Checks the timer has not already been started and initiates a coroutine
    [Command]
    public void CmdStartMissileTimer()
    {
        float time = 60f;
        if (!missileStarted)
        {
            missileStarted = true;
            StartCoroutine(MissileTimer(time));
        }
    }

    // Master coroutine only run on the server which then updates time left to all clients
    IEnumerator MissileTimer(float time)
    {
        float currentTime = time;
        UpdateMissileTimer(currentTime);
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1f);
            currentTime--;
            UpdateMissileTimer(currentTime);
        }
        CallChangeCameras(false);
        if (!gameEnded)
        {
            myNetworkManager.ServerChangeScene("EndGameLose");
        }
    }

    // Relays amount of time left to all clients every second
    [ClientRpc]
    public void UpdateMissileTimer(float time)
    {
        if (missileManager == null)
        {
            print("Missile manager not set");
        }
        else
        {
            missileManager.GetComponent<MissileTextManager>().UpdateTime(time);
        }
    }
    #endregion

    // Networking for the main game timer
    #region:Timer

    // Coroutine to count down the time and update the clients at regular intervals
    IEnumerator masterTimer()
    {
        masterTime = 0f;
        while (masterTime < time)
        {
            UpdateTime();
            masterTime += (time / increments);
            yield return new WaitForSeconds(time / increments);
        }
        // Players are destroyed at the end of the game
        DestroyAllPlayers();
        // If the timer ends and the game has not already ended then it changes to the lose screen
        if (!gameEnded)
        {
            myNetworkManager.ServerChangeScene("EndGameLose");
        }
    }

    // Relays time to all players
    [ClientRpc]
    private void UpdateTime()
    {
        if (timerStarted)
        {
            if (timerManager == null)
            {
                print("Timer manager not set");
            }
            else
            {
                timerManager.UpdateTimer(masterTime, time, increments);
            }
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
    }

    [ClientRpc]
    public void CallUpdateServerButtonPressed(GameObject button)
    {
        button.GetComponent<ResetServerButton>().ResetButtonPressed();
    }



    #endregion

    // Networking for tasks in the orientation phase
    // This is currently limited but our vision was to include more of a tutorial
    // within the orientation
    #region Orientation Minigames
    
    // Called by a player when they interact with the dinner plate
    public void DinnerPlate(GameObject interactable)
    {
        CmdDinnerPlate(interactable);
    }

    [Command]
    public void CmdDinnerPlate(GameObject interactable)
    {
        CallDinnerPlate(interactable);
        // Checks if a player has already interacted
        if (!orientationEnded)
        {
            // If not it starts the cut scene
            StartCoroutine(FinishOrientationTime());
        }
        else
        {
            orientationEnded = true;
        }
    }

    // Removes dinner plate for all players so it cannot be pressed again
    [ClientRpc]
    public void CallDinnerPlate(GameObject interactable)
    {
        interactable.SetActive(false);
    }
    #endregion

    // Networking for the opening of the doors below the control room
    #region Doors

    // All functions are fairly self-explanatory!

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

    // Networking for the captain command line
    #region:CommandLine

    // Used in orientation time to make sure the networked aspect of the command line spawns
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

    // Called by a client to queue a command that all players need to see
    public void WriteCommand(GameObject commandNetwork, string msg, bool captain = false)
    {
        CmdWriteCommand(commandNetwork, msg, captain);
    }
    [Command]
    public void CmdWriteCommand(GameObject commandNetwork, string msg, bool captain)
    {
        CallNetworkQueueMessage(commandNetwork, msg, captain);
    }

    // Relays a command to all clients by queueing it
    [ClientRpc]
    public void CallNetworkQueueMessage(GameObject commandNetwork, string msg, bool captain)
    {
        commandNetwork.GetComponent<CommandNetworkManager>().QueueNetworkMessage(msg, captain);
    }
    #endregion

    // Networking for the end game scenes
    #region EndGameScenes

    // Destroys all players on all clients
    [ClientRpc]
    public void DestroyAllPlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player != null)
            {
                Destroy(player);
            }
        }
    }

    // Used for ensuring multiple players don't try and end the game
    // Or so that the timer doesn't run out if the game has already ended
    [Command]
    public void CmdEndGame()
    {
        gameEnded = true;
    }
    #endregion
}
