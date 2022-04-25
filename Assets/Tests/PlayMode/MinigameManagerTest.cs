using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TMPro;

public class MiniGameManagerTest
{
    [UnityTest]
    public IEnumerator AddingObjective()
    {
        GameObject gameObject = new GameObject();
        gameObject.AddComponent<MinigameManager>();

        //Player UI initialisation
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
        //End of Player UI initialisation

        gameObject.GetComponent<MinigameManager>().UIManager = UIManager;

        gameObject.GetComponent<MinigameManager>().SendObjectiveData("test", "test");

        yield return null;

        Dictionary<string, string> allObjectives = new Dictionary<string, string>();
        allObjectives.Add("test", "test");
        Assert.AreEqual(allObjectives, gameObject.GetComponent<MinigameManager>().allObjectives);
    }

    [UnityTest]
    public IEnumerator CompletingObjectiveCorrectDictionaries()
    {
        GameObject gameObject = new GameObject();
        gameObject.AddComponent<MinigameManager>();

        //Player UI initialisation
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
        //End of Player UI initialisation

        gameObject.GetComponent<MinigameManager>().UIManager = UIManager;

        GameObject commandLine = new GameObject("Command Line", typeof(RectTransform));
        //commandLine.AddComponent<RectTransform>();
        commandLine.AddComponent<TextMeshProUGUI>();
        commandLine.AddComponent<CommandManager>();

        gameObject.GetComponent<MinigameManager>().commandLine = commandLine;

        GameObject ObjectiveCompleteGif = new GameObject();
        ObjectiveCompleteGif.AddComponent<UISpritesAnimation>();
        ObjectiveCompleteGif.AddComponent<Image>();
        gameObject.GetComponent<MinigameManager>().ObjectiveCompleteGif = ObjectiveCompleteGif;
        gameObject.GetComponent<MinigameManager>().objectiveStatusPopUpTime = 1;

        GameObject ObjectiveStatusUI = new GameObject();
        gameObject.GetComponent<MinigameManager>().ObjectiveStatusUI = ObjectiveStatusUI;

        gameObject.GetComponent<MinigameManager>().SendObjectiveData("test", "test");
        gameObject.GetComponent<MinigameManager>().ObjectiveCompleted("test", "test");

        yield return null;

        Dictionary<string, string> currentObjectives = new Dictionary<string, string>();
        Dictionary<string, string> doneObjectives = new Dictionary<string, string>();
        doneObjectives.Add("test", "test");

        Assert.AreEqual(currentObjectives, gameObject.GetComponent<MinigameManager>().GetCurrentObjectives());
        Assert.AreEqual(doneObjectives, gameObject.GetComponent<MinigameManager>().GetDoneObjectives());
    }

    [UnityTest]
    public IEnumerator CompletingObjectiveUIs()
    {
        GameObject gameObject = new GameObject();
        gameObject.AddComponent<MinigameManager>();

        //Player UI initialisation
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
        //End of Player UI initialisation

        gameObject.GetComponent<MinigameManager>().UIManager = UIManager;

        GameObject commandLine = new GameObject("Command Line", typeof(RectTransform));
        //commandLine.AddComponent<RectTransform>();
        commandLine.AddComponent<TextMeshProUGUI>();
        commandLine.AddComponent<CommandManager>();

        gameObject.GetComponent<MinigameManager>().commandLine = commandLine;

        GameObject ObjectiveCompleteGif = new GameObject();
        ObjectiveCompleteGif.AddComponent<UISpritesAnimation>();
        ObjectiveCompleteGif.AddComponent<Image>();
        gameObject.GetComponent<MinigameManager>().ObjectiveCompleteGif = ObjectiveCompleteGif;
        gameObject.GetComponent<MinigameManager>().objectiveStatusPopUpTime = 1;

        GameObject ObjectiveStatusUI = new GameObject();
        gameObject.GetComponent<MinigameManager>().ObjectiveStatusUI = ObjectiveStatusUI;

        gameObject.GetComponent<MinigameManager>().SendObjectiveData("test", "test");
        gameObject.GetComponent<MinigameManager>().ObjectiveCompleted("test", "test");

        gameObject.GetComponent<MinigameManager>().ObjectiveStatusUI.SetActive(false);
        gameObject.GetComponent<MinigameManager>().ObjectiveCompleteGif.SetActive(false);

        gameObject.GetComponent<MinigameManager>().ShowSuccess("test");
        Assert.AreEqual(true, gameObject.GetComponent<MinigameManager>().ObjectiveStatusUI.activeSelf);
        Assert.AreEqual(true, gameObject.GetComponent<MinigameManager>().ObjectiveCompleteGif.activeSelf);
        yield return new WaitForSeconds(1);

        Assert.AreEqual(false, gameObject.GetComponent<MinigameManager>().ObjectiveStatusUI.activeSelf);
    }

    [UnityTest]
    public IEnumerator CompletingObjectiveEndgameActive()
    {
        GameObject gameObject = new GameObject();
        gameObject.AddComponent<MinigameManager>();

        //Player UI initialisation
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
        //End of Player UI initialisation

        gameObject.GetComponent<MinigameManager>().UIManager = UIManager;

        GameObject commandLine = new GameObject("Command Line", typeof(RectTransform));
        //commandLine.AddComponent<RectTransform>();
        commandLine.AddComponent<TextMeshProUGUI>();
        commandLine.AddComponent<CommandManager>();

        gameObject.GetComponent<MinigameManager>().commandLine = commandLine;

        GameObject ObjectiveCompleteGif = new GameObject();
        ObjectiveCompleteGif.AddComponent<UISpritesAnimation>();
        ObjectiveCompleteGif.AddComponent<Image>();
        gameObject.GetComponent<MinigameManager>().ObjectiveCompleteGif = ObjectiveCompleteGif;
        gameObject.GetComponent<MinigameManager>().objectiveStatusPopUpTime = 1;

        GameObject ObjectiveStatusUI = new GameObject();
        gameObject.GetComponent<MinigameManager>().ObjectiveStatusUI = ObjectiveStatusUI;

        GameObject caps = new GameObject();
        gameObject.GetComponent<MinigameManager>().Caps = caps;

        gameObject.GetComponent<MinigameManager>().SendObjectiveData("test", "test");
        gameObject.GetComponent<MinigameManager>().ObjectiveCompleted("test", "test");

        yield return null;

        Assert.AreEqual(false, gameObject.GetComponent<MinigameManager>().GetEndgame());
    }

    [UnityTest]
    public IEnumerator CompletingObjectiveEndgameInactive()
    {
        GameObject gameObject = new GameObject();
        gameObject.AddComponent<MinigameManager>();

        //Player UI initialisation
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
        //End of Player UI initialisation

        gameObject.GetComponent<MinigameManager>().UIManager = UIManager;

        GameObject commandLine = new GameObject("Command Line", typeof(RectTransform));
        //commandLine.AddComponent<RectTransform>();
        commandLine.AddComponent<TextMeshProUGUI>();
        commandLine.AddComponent<CommandManager>();

        gameObject.GetComponent<MinigameManager>().commandLine = commandLine;

        GameObject ObjectiveCompleteGif = new GameObject();
        ObjectiveCompleteGif.AddComponent<UISpritesAnimation>();
        ObjectiveCompleteGif.AddComponent<Image>();
        gameObject.GetComponent<MinigameManager>().ObjectiveCompleteGif = ObjectiveCompleteGif;
        gameObject.GetComponent<MinigameManager>().objectiveStatusPopUpTime = 1;

        GameObject ObjectiveStatusUI = new GameObject();
        gameObject.GetComponent<MinigameManager>().ObjectiveStatusUI = ObjectiveStatusUI;

        GameObject caps = new GameObject();
        gameObject.GetComponent<MinigameManager>().Caps = caps;

        gameObject.GetComponent<MinigameManager>().SendObjectiveData("test", "test");
        gameObject.GetComponent<MinigameManager>().CheckWon();

        yield return null;

        Assert.AreEqual(true, gameObject.GetComponent<MinigameManager>().GetEndgame());
    }
}