using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonitorManager : MonoBehaviour
{
    public Color objectiveColour;
    public Color doneObjectiveColour;
    private ObjectiveMonitorManager objectiveMonitorManager;

    private void Awake()
    {
        objectiveMonitorManager = GetComponent<ObjectiveMonitorManager>();
    }
    public void UpdateObjectives(Dictionary<string, string> objectives, Dictionary<string, string> doneObjectives)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).GetChild(0).GetComponent<TextMeshPro>().ClearMesh();
        }
        int monitorCount = 1;
        foreach (var o in objectives)
        {
            switch (o.Key)
            {
                case "Engine needs backup power":
                    objectiveMonitorManager.UpdateObjectiveMonitor("Engine", o.Value);
                    break;
                case "Reactor needs stabilising":
                    objectiveMonitorManager.UpdateObjectiveMonitor("Reactor", o.Value);
                    break;
                case "Servers need resetting":
                    objectiveMonitorManager.UpdateObjectiveMonitor("Server", o.Value);
                    break;
                case "Missiles":
                    objectiveMonitorManager.UpdateObjectiveMonitor("Missile", o.Value);
                    break;
                default:
                    break;
            }
            if (o.Key != "Missiles")
            {
                gameObject.transform.GetChild(monitorCount).GetChild(0).GetComponent<TextMeshPro>().faceColor = objectiveColour;
                gameObject.transform.GetChild(monitorCount).GetChild(0).GetComponent<TextMeshPro>().text = o.Key;
                monitorCount += 1;
            }
        }
        foreach (var o in doneObjectives)
        {
            switch (o.Key)
            {
                case "Engine needs backup power":
                    objectiveMonitorManager.UpdateObjectiveMonitor("Engine", o.Value);
                    break;
                case "Reactor needs stabilising":
                    objectiveMonitorManager.UpdateObjectiveMonitor("Reactor", o.Value);
                    break;
                case "Servers need resetting":
                    objectiveMonitorManager.UpdateObjectiveMonitor("Server", o.Value);
                    break;
                case "Missiles":
                    objectiveMonitorManager.UpdateObjectiveMonitor("Missiles", o.Value);
                    break;
                default:
                    break;
            }
            if (o.Key != "Missiles")
            {
                gameObject.transform.GetChild(monitorCount).GetChild(0).GetComponent<TextMeshPro>().faceColor = doneObjectiveColour;
                gameObject.transform.GetChild(monitorCount).GetChild(0).GetComponent<TextMeshPro>().text = o.Key;
                monitorCount += 1;
            }
        }
    }
}
