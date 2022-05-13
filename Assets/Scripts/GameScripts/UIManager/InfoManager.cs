using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class InfoManager : MonoBehaviour
{
    /*
        SCRIPT FOR MAKING THE INFORMATION APPEAR AT THE BEGINNING
        OF THE ORIENTATION SCENE

        Contributors: Sam Barnes-Thornton
    */ 
    private RectTransform rectTransform;
    public GameObject text;
    private RectTransform textRectTransform;
    public GameObject button;
    private RectTransform buttonRectTransform;
    private TextMeshProUGUI textMesh;
    public GameObject playerUI;
    private GameObject[] players;
    public GameObject orientationSetup;

    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        Cursor.lockState = CursorLockMode.None;
        rectTransform = GetComponent<RectTransform>();
        textRectTransform = text.GetComponent<RectTransform>();
        buttonRectTransform = button.GetComponent<RectTransform>();
        textMesh = text.GetComponent<TextMeshProUGUI>();
        playerUI.SetActive(false);
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                players[i].GetComponent<PlayerManager>().FirstPersonCamera.GetComponent<FirstPersonController>().cameraEnabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Dynamically sizes box and text for screen resolution
        rectTransform.sizeDelta = new Vector2(Screen.width / 2.5f, Screen.height / 2);
        textRectTransform.localPosition = new Vector2(0, 0);
        textRectTransform.sizeDelta = new Vector2(0.9f * Screen.width / 3, 0.75f * Screen.height / 2);
        buttonRectTransform.localPosition = new Vector2(0, -Screen.height / 8);
        buttonRectTransform.sizeDelta = new Vector2(Screen.width / 10, Screen.height / 20);
        textMesh.fontSize = Screen.height * 0.04f;
    }

    // Called when the play button is pressed to remove the info page
    public void ExitInfo()
    {
        // Brings player state back to normal for the game
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                players[i].GetComponent<PlayerManager>().FirstPersonCamera.GetComponent<FirstPersonController>().cameraEnabled = true;
            }
        }
        Cursor.lockState = CursorLockMode.Locked;
        playerUI.SetActive(true);
        gameObject.SetActive(false);
        orientationSetup.GetComponent<OrientationSetup>().CaptainIntro();
    }
}
