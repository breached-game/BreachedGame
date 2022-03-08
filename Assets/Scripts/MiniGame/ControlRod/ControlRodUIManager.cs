using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ControlRodUIManager : MonoBehaviour
{
    public GameObject UICanvas;
    public GameObject controlRodCamera;

    // NEEDS TO BE NETWORK SYNCED - HAVE NO IDEA HOW TO DO THAT
    public void UpButton()
    {

    }

    public void DownButton()
    {

    }

    public void RightButton()
    {

    }

    public void LeftButton()
    {

    }

    public void ForwardButton()
    {

    }

    public void BackwardButton()
    {

    }

    public void QuitButton()
    {
        Cursor.lockState = CursorLockMode.Locked;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                players[i].GetComponent<PlayerManager>().FirstPersonCamera.SetActive(true);
                //TRASH CODING PRACTICE INBUILT SPEED
                players[i].GetComponent<PlayerManager>().Speed = 4.0f;
                controlRodCamera.SetActive(false);
                UICanvas.SetActive(false);
                break;
            }
        }
    }
}
