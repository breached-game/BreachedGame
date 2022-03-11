using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonitorManager : MonoBehaviour
{
    public Color objectiveColour;
    public Color doneObjectiveColour;

    public void UpdateObjectives(Dictionary<string, string> objectives, Dictionary<string, string> doneObjectives)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).GetChild(0).GetComponent<TextMeshPro>().ClearMesh();
        }
        int monitorCount = 0;
        foreach (var o in objectives)
        {
            gameObject.transform.GetChild(monitorCount).GetChild(0).GetComponent<TextMeshPro>().faceColor = objectiveColour;
            gameObject.transform.GetChild(monitorCount).GetChild(0).GetComponent<TextMeshPro>().text = o.Key + ":\n" + o.Value;
            monitorCount += 1;
        }
        foreach (var o in doneObjectives)
        {
            gameObject.transform.GetChild(monitorCount).GetChild(0).GetComponent<TextMeshPro>().faceColor = doneObjectiveColour;
            gameObject.transform.GetChild(monitorCount).GetChild(0).GetComponent<TextMeshPro>().text = o.Key + ":\n" + o.Value;
            monitorCount += 1;
        }
    }
}
