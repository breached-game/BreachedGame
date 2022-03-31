using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.Rendering;

public class BrightnessManager : MonoBehaviour
{
    private RectTransform backgroundTransform;
    public GameObject background;
    public GameObject exitButton;
    public GameObject crosshair;
    public GameObject slider;
    private bool show = true;
    // Start is called before the first frame update
    void Start()
    {
        slider.SetActive(true);
        crosshair.SetActive(false);
        backgroundTransform = background.GetComponent<RectTransform>();
        exitButton.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 8, Screen.height / 20);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                players[i].GetComponent<PlayerManager>().FirstPersonCamera.GetComponent<FirstPersonController>().cameraEnabled = false;
            }
        }
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (show)
        {
            backgroundTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
            Cursor.lockState = CursorLockMode.None;
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

    public void SetBrightness(float brightness)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                players[i].GetComponent<PlayerManager>().FirstPersonCamera.GetComponent<Volume>().profile.components[0].parameters[0].SetValue(new FloatParameter(brightness));
            }
        }
    }

    public void ExitBrightness()
    {
        show = false;
        Cursor.lockState = CursorLockMode.Locked;
        crosshair.SetActive(true);
        exitButton.SetActive(false);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                players[i].GetComponent<PlayerManager>().FirstPersonCamera.GetComponent<FirstPersonController>().cameraEnabled = true;
            }
        }
        background.SetActive(false);
        slider.SetActive(false);
    }
}
