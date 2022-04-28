using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Mirror;

public class SettingsMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject BackwardsButton;
    public GameObject QuitButton;
    public GameObject Logo;
    public GameObject Sensitivity;
    public GameObject crosshair;
    public GameObject brightness;

    private void Start()
    {
        RectTransform mainMenuTransform = mainMenu.GetComponent<RectTransform>();
        mainMenuTransform.sizeDelta = new Vector2(Screen.width / 3, Screen.height * 2/ 3);
        RectTransform BackwardButton = BackwardsButton.GetComponent<RectTransform>();
        BackwardButton.sizeDelta = new Vector2(mainMenuTransform.rect.width * 3 / 4, mainMenuTransform.rect.height / 8);
        BackwardButton.localPosition = new Vector3(0, -1* mainMenuTransform.rect.height / 4, 0);
        RectTransform QuitButtonTransform = QuitButton.GetComponent<RectTransform>();
        QuitButtonTransform.sizeDelta = new Vector2(mainMenuTransform.rect.width * 3 / 4, mainMenuTransform.rect.height / 8);
        QuitButtonTransform.localPosition = new Vector3(0, -1 * mainMenuTransform.rect.height * 3 / 8, 0);
        RectTransform LogoTransform = Logo.GetComponent<RectTransform>();
        LogoTransform.sizeDelta = new Vector2(mainMenuTransform.rect.width / 2, mainMenuTransform.rect.height / 2);
        LogoTransform.localPosition = new Vector3(0, mainMenuTransform.rect.height / 4, 0);
        RectTransform SensitivityTransform = Sensitivity.GetComponent<RectTransform>();
        SensitivityTransform.sizeDelta = new Vector2(mainMenuTransform.rect.width / 2, mainMenuTransform.rect.height / 16);
        SensitivityTransform.localPosition = new Vector3(0, -1 * mainMenuTransform.rect.height / 32, 0);
        RectTransform brightnessTransform = brightness.GetComponent<RectTransform>();
        brightnessTransform.sizeDelta = new Vector2(mainMenuTransform.rect.width / 2, mainMenuTransform.rect.height / 16);
        brightnessTransform.localPosition = new Vector3(0, -1 * mainMenuTransform.rect.height / 7, 0);

        //Remembering settings already input 
        if (PlayerPrefs.GetFloat("Sensitivity") != 0) Sensitivity.GetComponent<UnityEngine.UI.Slider>().value = PlayerPrefs.GetFloat("Sensitivity");
        brightness.GetComponent<UnityEngine.UI.Slider>().value = PlayerPrefs.GetFloat("Brightness");
    }
    //HARD CODED TRY TO FIND PLAYER FROM ALL PLAYERS - LOOK FOR BETTER SOLUTION
    public void SetMouseSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                players[i].GetComponent<PlayerManager>().FirstPersonCamera.GetComponent<FirstPersonController>().sensitivity = sensitivity;
            }
        }
    }

    public void SetBrightness(float brightness)
    {
        ColorAdjustments c;
        PlayerPrefs.SetFloat("Brightness", brightness);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                players[i].GetComponent<PlayerManager>().FirstPersonCamera.GetComponent<Volume>().profile.TryGet(out c);
                c.postExposure.value = brightness;
                players[i].GetComponent<PlayerManager>().FirstPersonCamera.GetComponent<Volume>().profile.Reset();
            }
        }
    }

    public void BackButton()
    {
        mainMenu.SetActive(false);
        crosshair.SetActive(true);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
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
