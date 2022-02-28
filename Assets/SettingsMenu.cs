using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class SettingsMenu : MonoBehaviour
{
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
}
