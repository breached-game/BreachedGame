using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    public GameObject prefabObjectiveName;
    public GameObject prefabObjectiveDescription;

    private List<GameObject> UIComponents = new List<GameObject>();

    private Vector3 componentPostion = new Vector3(5f, -5f, 0);
    

    private void Start()
    {
        componentPostion += gameObject.transform.position;

    }
    public void updateObjectiveList(Dictionary<string, string> objectives)
    {
        foreach(var objective in objectives)
        {
            string objectiveName = objective.Key;
            string objectiveDescription = objective.Value;
            createObjectiveBox(objectiveName, objectiveDescription);

        }

    }

    private void setPosition(GameObject UIElement)
    {
        if (UIComponents.Count == 0)
        {
            UIElement.transform.position = componentPostion;
        }
        else
        {

            float yOffset = UIComponents[UIComponents.Count - 1].GetComponent<Rect>().height / 2;
            Debug.Log(yOffset);
            componentPostion.y -= yOffset;
            UIElement.transform.position = componentPostion;

        }

    }

    private void createObjectiveBox(string objectiveName, string objectiveDescription)
    {
        GameObject objectiveNameUI = Instantiate(prefabObjectiveName, transform.position, Quaternion.identity);
        objectiveNameUI.transform.parent = gameObject.transform;
        objectiveNameUI.GetComponent<TextMeshProUGUI>().text = objectiveName;
        setPosition(objectiveNameUI);
        UIComponents.Add(objectiveNameUI);
    

       


        GameObject objectiveDescriptionUI = Instantiate(prefabObjectiveDescription, transform.position, Quaternion.identity);
        objectiveDescriptionUI.transform.parent = gameObject.transform;
        objectiveDescriptionUI.GetComponent<TextMeshProUGUI>().text = objectiveDescription;
        setPosition(objectiveDescriptionUI);
        UIComponents.Add(objectiveDescriptionUI);
    
    }
}
