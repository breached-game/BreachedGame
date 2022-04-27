using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 position = new Vector2();
    private Vector2 sizeDelta = new Vector2();

    private void Awake()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    private void Update()
    {
        position.x = 1.5f * (Screen.width / 16);
        position.y = 1.5f * (Screen.height / 10);
        sizeDelta.x = Screen.width / 8;
        sizeDelta.y = Screen.height / 5;
        rectTransform.position = position;
        rectTransform.sizeDelta = sizeDelta;
    }
}
