using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class WaterTimerManagerTest
{
    float waterGridTimer = 0.5f;
    GameObject waterTimer;
    GameObject minigameManager;
    GameObject uiManager;
    GameObject objectiveStatusUI;
    GameObject objectiveCompleteGif;
    GameObject commandLine;
    GameObject caps;
    GameObject commandNetwork;

    GameObject playerHoldingText;
    GameObject playerHoldingDesc;
    GameObject prefabObjectiveName;
    GameObject prefabObjectiveDescription;
    GameObject mainMenu;
    GameObject crosshair;
    GameObject monitorsGameObject;
    GameObject monitorChild0;
    GameObject monitorChild1;
    GameObject monitorParent;
    GameObject child0;
    GameObject child1;
    GameObject parent0;
    GameObject parent1;

    GameObject hudOverlay;
    GameObject waterGrid;
    GameObject waterGridWait;
    GameObject item;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        #region CommandLineObjects
        //Instantiating hudOverlay
        hudOverlay = new GameObject("hudOverlay", typeof(RectTransform));
        hudOverlay.AddComponent<Image>();
        #endregion CommandLineObjects

        #region PlayerUIGameObjects
        //Instantiating monitors
        monitorsGameObject = new GameObject();
        monitorsGameObject.SetActive(false);

        //Initialising another monitor parent 0
        parent0 = new GameObject();
        parent0.transform.SetParent(monitorsGameObject.transform);

        //Initialising another monitor parent 1
        parent1 = new GameObject();
        parent1.transform.SetParent(monitorsGameObject.transform);

        //Initialising another monitor child 0
        child0 = new GameObject();
        child0.AddComponent<TextMeshPro>();
        child0.transform.SetParent(parent0.transform);

        //Initialising another monitor child 1
        child1 = new GameObject();
        child1.AddComponent<TextMeshPro>();
        child1.transform.SetParent(parent1.transform);

        //Initialising monitor parent
        monitorParent = new GameObject();
        monitorParent.AddComponent<TextMeshProUGUI>();

        //Instantiating monitorChild1
        monitorChild1 = new GameObject();
        monitorChild1.AddComponent<TextMeshProUGUI>();

        //Instantiating monitorChild0
        monitorChild0 = new GameObject();
        monitorChild0.AddComponent<TextMeshProUGUI>();

        monitorChild0.transform.SetParent(monitorParent.transform);
        monitorChild1.transform.SetParent(monitorParent.transform);

        //Adding correct components to monitor manager
        monitorsGameObject.AddComponent<MonitorManager>();

        monitorsGameObject.AddComponent<ObjectiveMonitorManager>();
        monitorsGameObject.GetComponent<ObjectiveMonitorManager>().EngineMonitor = monitorParent;
        monitorsGameObject.GetComponent<ObjectiveMonitorManager>().ReactorMonitor = monitorParent;
        monitorsGameObject.GetComponent<ObjectiveMonitorManager>().ServerMonitor = monitorParent;
        monitorsGameObject.GetComponent<ObjectiveMonitorManager>().MissileMonitor = monitorParent;

        //Instantiating crosshair
        crosshair = new GameObject();

        //Instantiating mainMenu
        mainMenu = new GameObject();

        //Instantiating prefabObjectiveDescription
        prefabObjectiveDescription = new GameObject();

        //Instantiating prefabObjectiveName
        prefabObjectiveName = new GameObject();

        //Instantiating playerHoldingDesc
        playerHoldingDesc = new GameObject("PlayerHoldingDesc", typeof(RectTransform));
        playerHoldingDesc.AddComponent<TextMeshProUGUI>();

        //Instantiating playerHoldingText
        playerHoldingText = new GameObject("PlayerHoldingText", typeof(RectTransform));
        playerHoldingText.AddComponent<TextMeshProUGUI>();
        #endregion PlayerUIObjects

        #region MinigameManagerObjects
        //Instantiating Caps
        caps = new GameObject();

        //Instantiating commandLine
        commandLine = new GameObject("Command Line", typeof(RectTransform));
        commandLine.SetActive(false);
        commandLine.AddComponent<TextMeshProUGUI>();
        commandLine.AddComponent<CommandManager>();
        commandLine.GetComponent<CommandManager>().hudTrap = hudOverlay;
        commandLine.SetActive(true);
        yield return null;

        //Instantiating commandNetwork
        commandNetwork = new GameObject();
        commandNetwork.SetActive(false);
        commandNetwork.AddComponent<CommandNetworkManager>();
        commandNetwork.GetComponent<CommandNetworkManager>().commandObject = commandLine;
        commandNetwork.SetActive(true);
        yield return null;

        //Instantiating ObjectiveCompleteGif
        objectiveCompleteGif = new GameObject();
        objectiveCompleteGif.AddComponent<UISpritesAnimation>();
        objectiveCompleteGif.AddComponent<Image>();

        //Instantiating ObjectiveStatusUI
        objectiveStatusUI = new GameObject();

        //Instantiating PlayerUI - needs playerHoldingText, playerHoldingDesc, prefabObjectiveName, prefabObjectiveDescription, mainMenu, crosshair, monitors
        uiManager = new GameObject();
        uiManager.SetActive(false);
        uiManager.AddComponent<PlayerUIManager>();
        uiManager.GetComponent<PlayerUIManager>().playerHoldingText = playerHoldingText;
        uiManager.GetComponent<PlayerUIManager>().playerHoldingDesc = playerHoldingDesc;
        uiManager.GetComponent<PlayerUIManager>().prefabObjectiveName = prefabObjectiveName;
        uiManager.GetComponent<PlayerUIManager>().prefabObjectiveDescription = prefabObjectiveDescription;
        uiManager.GetComponent<PlayerUIManager>().mainMenu = mainMenu;
        uiManager.GetComponent<PlayerUIManager>().crosshair = crosshair;
        uiManager.GetComponent<PlayerUIManager>().monitors = monitorsGameObject.GetComponent<MonitorManager>();
        uiManager.SetActive(true);
        yield return null;

        //Instantiating MinigameManager - needs UIManager, ObjectiveStatusUI, ObjectiveCompleteGif, commandLine, Caps, commandNetwork
        minigameManager = new GameObject();
        minigameManager.SetActive(false);
        minigameManager.AddComponent<MinigameManager>();
        minigameManager.GetComponent<MinigameManager>().UIManager = uiManager;
        minigameManager.GetComponent<MinigameManager>().ObjectiveStatusUI = objectiveStatusUI;
        minigameManager.GetComponent<MinigameManager>().ObjectiveCompleteGif = objectiveCompleteGif;
        minigameManager.GetComponent<MinigameManager>().objectiveStatusPopUpTime = 1;
        minigameManager.GetComponent<MinigameManager>().commandLine = commandLine;
        minigameManager.GetComponent<MinigameManager>().Caps = caps;
        minigameManager.GetComponent<MinigameManager>().commandNetwork = commandNetwork;

        #endregion MinigameManager

        #region WaterTimerObjects
        //Initialising first water grid
        waterGrid = new GameObject();

        //Initialising second water grid
        waterGridWait = new GameObject();
        waterGridWait.SetActive(false);

        //Initialising item droppoed off
        item = new GameObject();

        //Initialising waterTimer
        waterTimer = new GameObject();
        waterTimer.SetActive(false);
        waterTimer.AddComponent<WaterTimerManager>();
        waterTimer.AddComponent<DropOffMiniGameManager>();

        //Adding Drop Off MiniGame Manager Variables
        waterTimer.GetComponent<DropOffMiniGameManager>().ItemDroppedOff = item;
        waterTimer.GetComponent<DropOffMiniGameManager>().active = false;
        waterTimer.GetComponent<DropOffMiniGameManager>().minigameName = "Test";
        waterTimer.GetComponent<DropOffMiniGameManager>().minigameObjective = "Test";
        //Finished adding Drop Off MiniGame Manager Variables
        waterTimer.transform.SetParent(minigameManager.transform);
        waterGridTimer = 0.5f;
        waterTimer.GetComponent<WaterTimerManager>().waterGridTimer = waterGridTimer;
        waterTimer.GetComponent<WaterTimerManager>().waterGrid = waterGrid;
        waterTimer.GetComponent<WaterTimerManager>().waterGridWait = waterGridWait;
        waterTimer.GetComponent<WaterTimerManager>().online = false;
        #endregion WaterTimerObjects

        yield return null;
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(minigameManager);
        Object.Destroy(uiManager);
        Object.Destroy(objectiveStatusUI);
        Object.Destroy(objectiveCompleteGif);
        Object.Destroy(commandLine);
        Object.Destroy(caps);
        Object.Destroy(commandNetwork);

        Object.Destroy(playerHoldingText);
        Object.Destroy(playerHoldingDesc);
        Object.Destroy(prefabObjectiveName);
        Object.Destroy(prefabObjectiveDescription);
        Object.Destroy(mainMenu);
        Object.Destroy(crosshair);
        Object.Destroy(monitorsGameObject);
        Object.Destroy(monitorChild0);
        Object.Destroy(monitorChild1);
        Object.Destroy(monitorParent);

        Object.Destroy(hudOverlay);
        Object.Destroy(child0);
        Object.Destroy(child1);
        Object.Destroy(parent0);
        Object.Destroy(parent1);

        Object.Destroy(waterTimer);
        Object.Destroy(waterGrid);
        Object.Destroy(waterGridWait);
        Object.Destroy(item);
    }

    [UnityTest]
    public IEnumerator TimerWorking()
    {
        minigameManager.SetActive(true);
        waterTimer.SetActive(true);
        yield return new WaitForSeconds(waterGridTimer + 0.1f); //Add offset

        Assert.AreEqual(false, waterTimer.GetComponent<WaterTimerManager>().GetRun());
    }

    [UnityTest]
    public IEnumerator WaterGridSetToActive()
    {
        minigameManager.SetActive(true);
        waterTimer.SetActive(true);
        yield return new WaitForSeconds(waterGridTimer + 0.1f); //Add offset

        Assert.AreEqual(true, waterTimer.GetComponent<WaterTimerManager>().waterGrid.activeSelf);
    }

    [UnityTest]
    public IEnumerator WoodIsSetToInactive()
    {
        minigameManager.SetActive(true);
        waterTimer.SetActive(true);
        yield return new WaitForSeconds(waterGridTimer + 0.1f); //Add offset

        Assert.AreEqual(false, waterTimer.GetComponent<DropOffMiniGameManager>().ItemDroppedOff.activeSelf);
    }

    [UnityTest]
    public IEnumerator NewObjectiveAdded()
    {
        minigameManager.SetActive(true);
        waterTimer.SetActive(true);
        yield return new WaitForSeconds(waterGridTimer + 0.1f); //Add offset

        Dictionary<string, string> allObjectives = new Dictionary<string, string>();
        allObjectives.Add("Test", "Test");
        Assert.AreEqual(allObjectives, minigameManager.GetComponent<MinigameManager>().allObjectives);
    }

    [UnityTest]
    public IEnumerator TimeHasNotExceededCheck()
    {
        waterTimer.GetComponent<WaterTimerManager>().waterGridTimer = 25f;
        minigameManager.SetActive(true);
        waterTimer.SetActive(true);
        yield return new WaitForSeconds(1f);

        Assert.AreEqual(true, waterTimer.GetComponent<WaterTimerManager>().GetRun());
    }

    [UnityTest]
    public IEnumerator PreviousWaterGridNotFinished()
    {
        waterTimer.GetComponent<WaterTimerManager>().waterGridTimer = 25f;
        minigameManager.SetActive(true);
        waterTimer.SetActive(true);
        yield return new WaitForSeconds(1f);

        Assert.AreEqual(false, waterTimer.GetComponent<WaterTimerManager>().waterGridWait.activeSelf);
    }
}
