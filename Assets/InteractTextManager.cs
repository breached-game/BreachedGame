using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InteractTextManager : MonoBehaviour
{
    private RectTransform rectTransform;
    private TextMeshProUGUI textMesh;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.position = new Vector2(Screen.width / 2, Screen.height / 3.5f);
        textMesh.fontSize = Screen.height / 25;
    }
}
