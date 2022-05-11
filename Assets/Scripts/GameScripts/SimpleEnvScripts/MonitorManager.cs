using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonitorManager : MonoBehaviour
{
    /*
        SCRIPT FOR MANAGING THE OBJECTIVE MONITORS IN THE CONTROL ROOM
        ALSO RELAYS OBJECTIVE INFO TO MONITORS IN OTHER ROOMS

        Contributors: Sam Barnes-Thornton
    */
    public Color objectiveColour;
    public Color doneObjectiveColour;
    private ObjectiveMonitorManager objectiveMonitorManager;

    private void Awake()
    {
        objectiveMonitorManager = GetComponent<ObjectiveMonitorManager>();
    }

    // Called by MinigameManager at the start of the game
    // and whenever an objective is completed
    public void UpdateObjectives(Dictionary<string, string> objectives, Dictionary<string, string> doneObjectives)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).GetChild(0).GetComponent<TextMeshPro>().ClearMesh();
        }
        int monitorCount = 1;
        // Goes through each uncompleted objective and writes it in red on the monitors
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
            // Misses out the missiles objective as it has to be completed last so doesn't need to be on monitors
            if (o.Key != "Missiles")
            {
                gameObject.transform.GetChild(monitorCount).GetChild(0).GetComponent<TextMeshPro>().faceColor = objectiveColour;
                gameObject.transform.GetChild(monitorCount).GetChild(0).GetComponent<TextMeshPro>().text = o.Key;
                monitorCount += 1;
            }
        }

        // Goes through each of the completed objectives and writes them in green on the monitors
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
            // Again misses out the missile objective
            if (o.Key != "Missiles")
            {
                gameObject.transform.GetChild(monitorCount).GetChild(0).GetComponent<TextMeshPro>().faceColor = doneObjectiveColour;
                gameObject.transform.GetChild(monitorCount).GetChild(0).GetComponent<TextMeshPro>().text = o.Key;
                monitorCount += 1;
            }
        }
    }
}
