using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

public class WaterTimerManagerTest
{
    [UnityTest]
    public IEnumerator TimerWorking()
    {
        GameObject parent = new GameObject();
        parent.AddComponent<MinigameManager>();

        //Player UI Script initialisation
        GameObject UIManager = new GameObject();
        UIManager.AddComponent<PlayerUIManager>();

        GameObject mainMenu = new GameObject();
        UIManager.GetComponent<PlayerUIManager>().mainMenu = mainMenu;
        GameObject crosshair = new GameObject();
        UIManager.GetComponent<PlayerUIManager>().crosshair = crosshair;

        GameObject monitors = new GameObject();
        monitors.AddComponent<MonitorManager>();
        GameObject monitor0 = new GameObject();
        GameObject monitor1 = new GameObject();
        monitor0.transform.SetParent(monitors.transform);
        monitor1.transform.SetParent(monitors.transform);

        GameObject monitor0Child = new GameObject();
        GameObject monitor1Child = new GameObject();
        monitor0Child.AddComponent<TextMeshPro>();
        monitor1Child.AddComponent<TextMeshPro>();

        monitor0Child.transform.SetParent(monitor0.transform);
        monitor1Child.transform.SetParent(monitor1.transform);

        UIManager.GetComponent<PlayerUIManager>().monitors = monitors.GetComponent<MonitorManager>();
        //End of Player UI Script initialisation

        parent.GetComponent<MinigameManager>().UIManager = UIManager;

        GameObject gameObject = new GameObject();
        gameObject.AddComponent<WaterTimerManager>();
        gameObject.AddComponent<DropOffMiniGameManager>();
        gameObject.transform.SetParent(parent.transform);

        float waterGridTimer = 1f;
        GameObject waterGrid = new GameObject();

        GameObject item = new GameObject();
        //Adding Drop Off MiniGame Manager Variables
        gameObject.GetComponent<DropOffMiniGameManager>().ItemDroppedOff = item;
        gameObject.GetComponent<DropOffMiniGameManager>().active = false;
        gameObject.GetComponent<DropOffMiniGameManager>().minigameName = "Test";
        gameObject.GetComponent<DropOffMiniGameManager>().minigameObjective = "Test";
        //Finished adding Drop Off MiniGame Manager Variables

        gameObject.GetComponent<WaterTimerManager>().waterGridTimer = waterGridTimer;
        gameObject.GetComponent<WaterTimerManager>().waterGrid = waterGrid;

        yield return new WaitForSeconds(waterGridTimer + 0.1f); //Add offset

        Assert.AreEqual(false, gameObject.GetComponent<WaterTimerManager>().GetRun());
    }

    [UnityTest]
    public IEnumerator WaterGridSetToActive()
    {
        GameObject parent = new GameObject();
        parent.AddComponent<MinigameManager>();

        //Player UI Script initialisation
        GameObject UIManager = new GameObject();
        UIManager.AddComponent<PlayerUIManager>();

        GameObject mainMenu = new GameObject();
        UIManager.GetComponent<PlayerUIManager>().mainMenu = mainMenu;
        GameObject crosshair = new GameObject();
        UIManager.GetComponent<PlayerUIManager>().crosshair = crosshair;

        GameObject monitors = new GameObject();
        monitors.AddComponent<MonitorManager>();
        GameObject monitor0 = new GameObject();
        GameObject monitor1 = new GameObject();
        monitor0.transform.SetParent(monitors.transform);
        monitor1.transform.SetParent(monitors.transform);

        GameObject monitor0Child = new GameObject();
        GameObject monitor1Child = new GameObject();
        monitor0Child.AddComponent<TextMeshPro>();
        monitor1Child.AddComponent<TextMeshPro>();

        monitor0Child.transform.SetParent(monitor0.transform);
        monitor1Child.transform.SetParent(monitor1.transform);

        UIManager.GetComponent<PlayerUIManager>().monitors = monitors.GetComponent<MonitorManager>();
        //End of Player UI Script initialisation

        parent.GetComponent<MinigameManager>().UIManager = UIManager;

        GameObject gameObject = new GameObject();
        gameObject.AddComponent<WaterTimerManager>();
        gameObject.AddComponent<DropOffMiniGameManager>();
        gameObject.transform.SetParent(parent.transform);

        float waterGridTimer = 1f;
        GameObject waterGrid = new GameObject();
        waterGrid.SetActive(false);

        GameObject item = new GameObject();
        //Adding Drop Off MiniGame Manager Variables
        gameObject.GetComponent<DropOffMiniGameManager>().ItemDroppedOff = item;
        gameObject.GetComponent<DropOffMiniGameManager>().active = false;
        gameObject.GetComponent<DropOffMiniGameManager>().minigameName = "Test";
        gameObject.GetComponent<DropOffMiniGameManager>().minigameObjective = "Test";
        //Finished adding Drop Off MiniGame Manager Variables

        gameObject.GetComponent<WaterTimerManager>().waterGridTimer = waterGridTimer;
        gameObject.GetComponent<WaterTimerManager>().waterGrid = waterGrid;

        yield return new WaitForSeconds(waterGridTimer + 0.1f); //Add offset

        Assert.AreEqual(true, gameObject.GetComponent<WaterTimerManager>().waterGrid.activeSelf);
    }

    [UnityTest]
    public IEnumerator WoodIsSetToInactive()
    {
        GameObject parent = new GameObject();
        parent.AddComponent<MinigameManager>();

        //Player UI Script initialisation
        GameObject UIManager = new GameObject();
        UIManager.AddComponent<PlayerUIManager>();

        GameObject mainMenu = new GameObject();
        UIManager.GetComponent<PlayerUIManager>().mainMenu = mainMenu;
        GameObject crosshair = new GameObject();
        UIManager.GetComponent<PlayerUIManager>().crosshair = crosshair;

        GameObject monitors = new GameObject();
        monitors.AddComponent<MonitorManager>();
        GameObject monitor0 = new GameObject();
        GameObject monitor1 = new GameObject();
        monitor0.transform.SetParent(monitors.transform);
        monitor1.transform.SetParent(monitors.transform);

        GameObject monitor0Child = new GameObject();
        GameObject monitor1Child = new GameObject();
        monitor0Child.AddComponent<TextMeshPro>();
        monitor1Child.AddComponent<TextMeshPro>();

        monitor0Child.transform.SetParent(monitor0.transform);
        monitor1Child.transform.SetParent(monitor1.transform);

        UIManager.GetComponent<PlayerUIManager>().monitors = monitors.GetComponent<MonitorManager>();
        //End of Player UI Script initialisation

        parent.GetComponent<MinigameManager>().UIManager = UIManager;

        GameObject gameObject = new GameObject();
        gameObject.AddComponent<WaterTimerManager>();
        gameObject.AddComponent<DropOffMiniGameManager>();
        gameObject.transform.SetParent(parent.transform);

        float waterGridTimer = 1f;
        GameObject waterGrid = new GameObject();

        GameObject item = new GameObject();
        //Adding Drop Off MiniGame Manager Variables
        gameObject.GetComponent<DropOffMiniGameManager>().ItemDroppedOff = item;
        gameObject.GetComponent<DropOffMiniGameManager>().active = false;
        gameObject.GetComponent<DropOffMiniGameManager>().minigameName = "Test";
        gameObject.GetComponent<DropOffMiniGameManager>().minigameObjective = "Test";
        //Finished adding Drop Off MiniGame Manager Variables

        gameObject.GetComponent<WaterTimerManager>().waterGridTimer = waterGridTimer;
        gameObject.GetComponent<WaterTimerManager>().waterGrid = waterGrid;

        yield return new WaitForSeconds(waterGridTimer + 0.1f); //Add offset

        Assert.AreEqual(false, gameObject.GetComponent<DropOffMiniGameManager>().ItemDroppedOff.activeSelf);
    }

    [UnityTest]
    public IEnumerator NewObjectiveAdded()
    {
        GameObject parent = new GameObject();
        parent.AddComponent<MinigameManager>();

        //Player UI Script initialisation
        GameObject UIManager = new GameObject();
        UIManager.AddComponent<PlayerUIManager>();

        GameObject mainMenu = new GameObject();
        UIManager.GetComponent<PlayerUIManager>().mainMenu = mainMenu;
        GameObject crosshair = new GameObject();
        UIManager.GetComponent<PlayerUIManager>().crosshair = crosshair;

        GameObject monitors = new GameObject();
        monitors.AddComponent<MonitorManager>();
        GameObject monitor0 = new GameObject();
        GameObject monitor1 = new GameObject();
        monitor0.transform.SetParent(monitors.transform);
        monitor1.transform.SetParent(monitors.transform);

        GameObject monitor0Child = new GameObject();
        GameObject monitor1Child = new GameObject();
        monitor0Child.AddComponent<TextMeshPro>();
        monitor1Child.AddComponent<TextMeshPro>();

        monitor0Child.transform.SetParent(monitor0.transform);
        monitor1Child.transform.SetParent(monitor1.transform);

        UIManager.GetComponent<PlayerUIManager>().monitors = monitors.GetComponent<MonitorManager>();
        //End of Player UI Script initialisation

        parent.GetComponent<MinigameManager>().UIManager = UIManager;

        GameObject gameObject = new GameObject();
        gameObject.AddComponent<WaterTimerManager>();
        gameObject.AddComponent<DropOffMiniGameManager>();
        gameObject.transform.SetParent(parent.transform);

        float waterGridTimer = 1f;
        GameObject waterGrid = new GameObject();

        GameObject item = new GameObject();
        //Adding Drop Off MiniGame Manager Variables
        gameObject.GetComponent<DropOffMiniGameManager>().ItemDroppedOff = item;
        gameObject.GetComponent<DropOffMiniGameManager>().active = false;
        gameObject.GetComponent<DropOffMiniGameManager>().minigameName = "Test";
        gameObject.GetComponent<DropOffMiniGameManager>().minigameObjective = "Test";
        //Finished adding Drop Off MiniGame Manager Variables

        gameObject.GetComponent<WaterTimerManager>().waterGridTimer = waterGridTimer;
        gameObject.GetComponent<WaterTimerManager>().waterGrid = waterGrid;

        yield return new WaitForSeconds(waterGridTimer + 0.1f); //Add offset

        Dictionary<string, string> allObjectives = new Dictionary<string, string>();
        allObjectives.Add("Test", "Test");
        Assert.AreEqual(allObjectives, parent.GetComponent<MinigameManager>().allObjectives);
    }

    [UnityTest]
    public IEnumerator TimeHasNotExceededCheck()
    {
        GameObject parent = new GameObject();
        parent.AddComponent<MinigameManager>();

        //Player UI Script initialisation
        GameObject UIManager = new GameObject();
        UIManager.AddComponent<PlayerUIManager>();

        GameObject mainMenu = new GameObject();
        UIManager.GetComponent<PlayerUIManager>().mainMenu = mainMenu;
        GameObject crosshair = new GameObject();
        UIManager.GetComponent<PlayerUIManager>().crosshair = crosshair;

        GameObject monitors = new GameObject();
        monitors.AddComponent<MonitorManager>();
        GameObject monitor0 = new GameObject();
        GameObject monitor1 = new GameObject();
        monitor0.transform.SetParent(monitors.transform);
        monitor1.transform.SetParent(monitors.transform);

        GameObject monitor0Child = new GameObject();
        GameObject monitor1Child = new GameObject();
        monitor0Child.AddComponent<TextMeshPro>();
        monitor1Child.AddComponent<TextMeshPro>();

        monitor0Child.transform.SetParent(monitor0.transform);
        monitor1Child.transform.SetParent(monitor1.transform);

        UIManager.GetComponent<PlayerUIManager>().monitors = monitors.GetComponent<MonitorManager>();
        //End of Player UI Script initialisation

        parent.GetComponent<MinigameManager>().UIManager = UIManager;

        GameObject gameObject = new GameObject();
        gameObject.AddComponent<WaterTimerManager>();
        gameObject.AddComponent<DropOffMiniGameManager>();
        gameObject.transform.SetParent(parent.transform);

        float waterGridTimer = 25f;
        GameObject waterGrid = new GameObject();

        GameObject item = new GameObject();
        //Adding Drop Off MiniGame Manager Variables
        gameObject.GetComponent<DropOffMiniGameManager>().ItemDroppedOff = item;
        gameObject.GetComponent<DropOffMiniGameManager>().active = false;
        gameObject.GetComponent<DropOffMiniGameManager>().minigameName = "Test";
        gameObject.GetComponent<DropOffMiniGameManager>().minigameObjective = "Test";
        //Finished adding Drop Off MiniGame Manager Variables

        gameObject.GetComponent<WaterTimerManager>().waterGridTimer = waterGridTimer;
        gameObject.GetComponent<WaterTimerManager>().waterGrid = waterGrid;

        yield return new WaitForSeconds(1f);

        Assert.AreEqual(true, gameObject.GetComponent<WaterTimerManager>().GetRun());
    }
}
