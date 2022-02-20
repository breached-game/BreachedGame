using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class SyncDictionaryStringString : SyncDictionary<string, string> { }

public class PlayerUIManager : NetworkBehaviour
{
    public GameObject playerHoldingText;
    public GameObject prefabObjectiveName;
    public GameObject prefabObjectiveDescription;

    public readonly SyncDictionaryStringString currentObjectives = new SyncDictionaryStringString();
    public readonly SyncDictionaryStringString doneObjectives = new SyncDictionaryStringString();

    public Color doneObjectTextColour;
    public Color objectTextColour;

    private List<GameObject> UIElements =  new List<GameObject>();
    private float offsetY = 5;

    public override void OnStartClient()
    {
        currentObjectives.Callback += OnObjectiveChange;
        doneObjectives.Callback += OnObjectiveChange;
    }

    public void UpdatePlayerHolding(string itemName)
    {
        playerHoldingText.GetComponent<TextMeshProUGUI>().text = itemName;
    }

    public void UpdateObjectiveUI()
    {
        //Clear UI
        offsetY = 5;
        foreach (GameObject UIElement in UIElements)
        {
            Destroy(UIElement);
        }
        UIElements.Clear();

        if (doneObjectives != null)
        {
            foreach (var objective in doneObjectives)
            {
                string objectiveName = objective.Key;
                string objectiveDescription = objective.Value;
                createObjectiveBox(objectiveName, objectiveDescription, doneObjectTextColour);
            }
        }

        foreach (var objective in currentObjectives)
        {
            string objectiveName = objective.Key;
            string objectiveDescription = objective.Value;
            createObjectiveBox(objectiveName, objectiveDescription, objectTextColour);
        }
    }
    public void updateObjectiveList(Dictionary<string, string> objectives, Dictionary<string, string> completedObjectives)
    {
        currentObjectives.Clear();
        doneObjectives.Clear();
        foreach (var objective in objectives)
        {
            currentObjectives.Add(objective);
        }
        foreach (var objective in completedObjectives)
        {
            doneObjectives.Add(objective);
        }
        UpdateObjectiveUI();
    }

    public void OnObjectiveChange(SyncDictionaryStringString.Operation op, string key, string value)
    {
        Debug.Log("Callback");
        Debug.Log("Current Objectives: " + currentObjectives.Count);
        Debug.Log("Done Objectives: " + doneObjectives.Count);
        switch (op)
        {
            case SyncIDictionary<string, string>.Operation.OP_ADD:
                UpdateObjectiveUI();
                break;
            case SyncIDictionary<string, string>.Operation.OP_CLEAR:
                break;
            case SyncIDictionary<string, string>.Operation.OP_REMOVE:
                break;
            case SyncIDictionary<string, string>.Operation.OP_SET:
                break;
            default:
                break;
        }
    }

    private void setPosition(RectTransform UIElement)
    {
        //Get this from parent //hard coded to be lazy
        float defultx = -100;
        float defulty = 100;
        UIElement.localPosition = new Vector3(defultx, defulty - offsetY, 0);
        UIElements.Add(UIElement.gameObject);
    }
    private void createObjectiveBox(string objectiveName, string objectiveDescription, Color colour)
    {
        GameObject objectiveNameUI = Instantiate(prefabObjectiveName, new Vector3(0,0,0), Quaternion.identity);
        objectiveNameUI.transform.SetParent(gameObject.transform, true);
        objectiveNameUI.GetComponent<TextMeshProUGUI>().text = objectiveName;
        objectiveNameUI.GetComponent<TextMeshProUGUI>().color = colour;

        //If first then don't update offset
        if(UIElements.Count != 0)
        {
            offsetY = offsetY + objectiveNameUI.GetComponent<RectTransform>().rect.height/2;
        }
        setPosition(objectiveNameUI.GetComponent<RectTransform>());

        GameObject objectiveDescriptionUI = Instantiate(prefabObjectiveDescription, new Vector3(0, 0, 0), Quaternion.identity);
        objectiveDescriptionUI.transform.SetParent(gameObject.transform, true);
        objectiveDescriptionUI.GetComponent<TextMeshProUGUI>().text = objectiveDescription;
        objectiveDescriptionUI.GetComponent<TextMeshProUGUI>().color = colour;

        offsetY = offsetY + objectiveDescriptionUI.GetComponent<RectTransform>().rect.height/2;
        setPosition(objectiveDescriptionUI.GetComponent<RectTransform>());

    }
}
