using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class PlayerUIManager : MonoBehaviour
{
    public GameObject playerHoldingText;
    public GameObject prefabObjectiveName;
    public GameObject prefabObjectiveDescription;
    public GameObject mainMenu;
    public GameObject crosshair;

    public Color doneObjectTextColour;
    public Color objectTextColour;

    private List<GameObject> UIElements =  new List<GameObject>();
    private float offsetY = 5;

    public void UpdatePlayerHolding(string itemName)
    {
        playerHoldingText.GetComponent<TextMeshProUGUI>().text = itemName;
    }
    public void updateObjectiveList(Dictionary<string, string> objectives, Dictionary<string, string> doneObjectives)
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

        foreach (var objective in objectives)
        {
            string objectiveName = objective.Key;
            string objectiveDescription = objective.Value;
            createObjectiveBox(objectiveName, objectiveDescription, objectTextColour);
        }
    }

    private void setPosition(RectTransform UIElement)
    {
        //Get this from parent //hard coded to be lazy
        float defaultx = -100;
        float defaulty = 100;
        UIElement.localPosition = new Vector3(defaultx, defaulty - offsetY, 0);
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

    public void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (!mainMenu.activeSelf)
            {
                mainMenu.SetActive(true);
                crosshair.SetActive(false);
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i].GetComponent<NetworkIdentity>().isLocalPlayer)
                    {
                        players[i].GetComponent<PlayerManager>().FirstPersonCamera.GetComponent<FirstPersonController>().cameraEnabled = false;
                    }
                }
            }
        }
    }

}
