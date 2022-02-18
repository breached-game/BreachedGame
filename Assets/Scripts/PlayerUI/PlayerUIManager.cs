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
    
    /*
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
            UIElement.GetComponent<RectTransform>().rect.Set(0, 0, 100, 100);
            UIElement.transform.position = componentPostion;
        }
        else
        {
            RectTransform rectOfUIElement = UIElement.GetComponent<RectTransform>();
            float yOffset = rectOfUIElement.rect.height / 2;
            Debug.Log(yOffset);
            rectOfUIElement.rect.Set(rectOfUIElement.rect.x, rectOfUIElement.rect.y - yOffset, rectOfUIElement.rect.width, rectOfUIElement.rect.height);
        }

    }

    private void createObjectiveBox(string objectiveName, string objectiveDescription)
    {
        GameObject objectiveNameUI = Instantiate(prefabObjectiveName, transform.position, Quaternion.identity);
        objectiveNameUI.transform.SetParent(gameObject.transform, false);
        objectiveNameUI.GetComponent<TextMeshProUGUI>().text = objectiveName;
        objectiveNameUI.GetComponent<RectTransform>().localPosition = new Vector3(5, -5, 0);
        //setPosition(objectiveNameUI);
        UIComponents.Add(objectiveNameUI);

        GameObject objectiveDescriptionUI = Instantiate(prefabObjectiveDescription, transform.position, Quaternion.identity);
        objectiveDescriptionUI.transform.SetParent(gameObject.transform, false);
        objectiveDescriptionUI.GetComponent<TextMeshProUGUI>().text = objectiveDescription;
        //setPosition(objectiveDescriptionUI);
        UIComponents.Add(objectiveDescriptionUI);
    
    }
    */
}
