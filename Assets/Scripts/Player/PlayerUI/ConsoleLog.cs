using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConsoleLog : MonoBehaviour
{
    public GameObject logItem;
    private List<GameObject> theLog = new List<GameObject>();
    public float offsetY = 5;
    public int logLimit = 2;

    private void setPosition(RectTransform UIElement)
    {
        //Get this from parent //hard coded to be lazy
        float defultx = -100;
        float defulty = 50;
        UIElement.localPosition = new Vector3(defultx, defulty - offsetY, 0);
        theLog.Add(UIElement.gameObject);
    }
    public void createLogItem(string logText)
    {
        if (theLog.Count == logLimit)
        {
            //Get rid of the oldest
            offsetY = offsetY - theLog[0].GetComponent<RectTransform>().rect.height / 2;
            Destroy(theLog[0]);
            theLog.RemoveAt(0);
        }
        if (theLog.Count >= logLimit) Debug.LogError("Log has too many messages to display");

        GameObject objectiveNameUI = Instantiate(logItem, new Vector3(0, 0, 0), Quaternion.identity);
        objectiveNameUI.transform.SetParent(gameObject.transform, true);
        objectiveNameUI.GetComponent<TextMeshProUGUI>().text = logText;

        //If first then don't update offset
        if (theLog.Count != 0)
        {
            offsetY = offsetY + objectiveNameUI.GetComponent<RectTransform>().rect.height / 2;
        }
        setPosition(objectiveNameUI.GetComponent<RectTransform>());

    }
}
