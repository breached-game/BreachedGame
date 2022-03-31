using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveMonitorManager : MonoBehaviour
{
    public GameObject EngineMonitor;
    public GameObject ReactorMonitor;
    public GameObject ServerMonitor;
    public GameObject MissileMonitor;
    private TextMeshPro EngineText;
    private TextMeshPro ReactorText;
    private TextMeshPro ServerText;
    private TextMeshPro MissileText;
    public Color infoColour;
    // Start is called before the first frame update
    void Awake()
    {
        EngineText = EngineMonitor.transform.GetChild(1).GetComponent<TextMeshPro>();
        ReactorText = ReactorMonitor.transform.GetChild(1).GetComponent<TextMeshPro>();
        ServerText = ServerMonitor.transform.GetChild(1).GetComponent<TextMeshPro>();
        MissileText = MissileMonitor.transform.GetChild(1).GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    public void UpdateObjectiveMonitor(string objectiveLoc, string objective)
    {
        switch (objectiveLoc)
        {
            case "Engine":
                EngineText.text = objective;
                EngineText.faceColor = infoColour;
                break;
            case "Reactor":
                ReactorText.text = objective;
                ReactorText.faceColor = infoColour;
                break;
            case "Server":
                ServerText.text = objective;
                ServerText.faceColor = infoColour;
                break;
            case "Missile":
                MissileText.text = objective;
                MissileText.faceColor = infoColour;
                break;
            default:
                break;
        }
    }
}
