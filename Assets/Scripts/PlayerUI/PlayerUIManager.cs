using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    public GameObject prefabObjectiveName;
    public GameObject prefabObjectiveDescription;

    private List<GameObject> UIElements =  new List<GameObject>();
    private float offsetY = 5;

    public void updateObjectiveList(Dictionary<string, string> objectives)
    {
        foreach(var objective in objectives)
        {
            string objectiveName = objective.Key;
            string objectiveDescription = objective.Value;
            createObjectiveBox(objectiveName, objectiveDescription);
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
    private void createObjectiveBox(string objectiveName, string objectiveDescription)
    {
        GameObject objectiveNameUI = Instantiate(prefabObjectiveName, new Vector3(0,0,0), Quaternion.identity);
        objectiveNameUI.transform.SetParent(gameObject.transform, true);
        objectiveNameUI.GetComponent<TextMeshProUGUI>().text = objectiveName;

        //If first then don't update offset
        if(UIElements.Count != 0)
        {
            offsetY = offsetY + objectiveNameUI.GetComponent<RectTransform>().rect.height/2;
        }
        setPosition(objectiveNameUI.GetComponent<RectTransform>());

        GameObject objectiveDescriptionUI = Instantiate(prefabObjectiveDescription, new Vector3(0, 0, 0), Quaternion.identity);
        objectiveDescriptionUI.transform.SetParent(gameObject.transform, true);
        objectiveDescriptionUI.GetComponent<TextMeshProUGUI>().text = objectiveDescription;

        offsetY = offsetY + objectiveDescriptionUI.GetComponent<RectTransform>().rect.height/2;
        setPosition(objectiveDescriptionUI.GetComponent<RectTransform>());

    }
}
