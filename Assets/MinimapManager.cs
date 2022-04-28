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
        position.x = 1.5f * (Screen.width / 12);
        position.y = 1.5f * (Screen.height / 6);
        sizeDelta.x = Screen.width / 6;
        sizeDelta.y = Screen.height / 3;
        rectTransform.position = position;
        rectTransform.sizeDelta = sizeDelta;
    }
}
