using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    /*
        SCRIPT FOR DYNAMICALLY SIZING PLAYER HEADS UP DISPLAY

        Contributors: Sam Barnes-Thornton
    */
    public GameObject PlayerUI;
    private RectTransform rectTransform;
    private RectTransform itemRectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        itemRectTransform = PlayerUI.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.position = new Vector2(Screen.width / 2, Screen.height / 16);
        rectTransform.sizeDelta = new Vector2(Screen.width / 10, Screen.height / 8f);
        itemRectTransform.position = new Vector2(Screen.width / 2, Screen.height / 16f);
    }
}
