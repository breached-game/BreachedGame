using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class PlayerUIManager : MonoBehaviour
{
    /*
        SCRIPT FOR MANAGING THE BARE BONES PLAYER USER
        INTERFACE: THE SETTINGS MENU AND ITEM DESCRIPTION

        Contributors: Andrew Morgan, Sam Barnes-Thornton and Seth Holdcroft
    */
    public GameObject playerHoldingText;
    public GameObject playerHoldingDesc;
    public GameObject prefabObjectiveName;
    public GameObject prefabObjectiveDescription;
    public GameObject mainMenu;
    public GameObject crosshair;
    public MonitorManager monitors;

    public Color doneObjectTextColour;
    public Color objectTextColour;

    GameObject[] players;
    GameObject playerCamera;

    private void Start()
    {
        // Sets the local player camera reference for easy use later on
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                playerCamera = player.GetComponent<PlayerManager>().FirstPersonCamera;
            }
        }
    }

    // Updates the description of the item the player is holding
    public void UpdatePlayerHolding(string itemName, string itemDesc)
    {
        playerHoldingText.GetComponent<TextMeshProUGUI>().text = itemName;
        playerHoldingDesc.GetComponent<TextMeshProUGUI>().text = itemDesc;
    }

    // Called by minigame manager to update the objectives that have been completed
    public void updateObjectiveList(Dictionary<string, string> objectives, Dictionary<string, string> doneObjectives)
    {
        monitors.UpdateObjectives(objectives, doneObjectives);
    }

    public void Update()
    {
        // Shows the settings menu when escape is pressed
        if (Input.GetKeyDown("escape"))
        {
            // Sets cursor to appear when the main menu is visible
            Cursor.lockState = CursorLockMode.None;
            if (!mainMenu.activeSelf)
            {
                if (mainMenu != null)
                {
                    mainMenu.SetActive(true);
                }
                crosshair.SetActive(false);
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i].GetComponent<NetworkIdentity>().isLocalPlayer)
                    {
                        // Stops the local player from being able to look around whilst browsing the settings menu
                        players[i].GetComponent<PlayerManager>().FirstPersonCamera.GetComponent<FirstPersonController>().cameraEnabled = false;
                    }
                }
            }
        }
        else
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    playerCamera = player.GetComponent<PlayerManager>().FirstPersonCamera;
                }
            }
        }
    }
}
