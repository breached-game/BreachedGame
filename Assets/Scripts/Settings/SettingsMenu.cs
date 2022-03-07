using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class SettingsMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject crosshair;


    //HARD CODED TRY TO FIND PLAYER FROM ALL PLAYERS - LOOK FOR BETTER SOLUTION
    public void SetMouseSensitivity(float sensitivity)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                players[i].GetComponent<PlayerManager>().FirstPersonCamera.GetComponent<FirstPersonController>().sensitivity = sensitivity;
            }
        }
    }

    public void BackButton()
    {
        mainMenu.SetActive(false);
        crosshair.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                players[i].GetComponent<PlayerManager>().FirstPersonCamera.GetComponent<FirstPersonController>().cameraEnabled = true;
            }
        }
    }
}
