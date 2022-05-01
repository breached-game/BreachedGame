using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    Slider slider;
    public GameObject timerTextObject;
    private Text timerText;
    public GameObject failScreen;
    private RectTransform rectTransform;
    private RectTransform textRectTransform;
    // Start is called before the first frame update
    void Awake()
    {
        slider = this.gameObject.GetComponent<Slider>();
        this.gameObject.SetActive(false);
        timerTextObject.SetActive(false);
        timerText = timerTextObject.GetComponent<Text>();
        rectTransform = GetComponent<RectTransform>();
        textRectTransform = timerTextObject.GetComponent<RectTransform>();
    }

    private void Update()
    {
        rectTransform.sizeDelta = new Vector2(2 * Screen.height / 3, Screen.width / 40);
        rectTransform.anchoredPosition = new Vector2(- 2* rectTransform.sizeDelta.y, 0);
        textRectTransform.anchoredPosition = new Vector2(-Screen.width/20, Screen.height/16);
        textRectTransform.sizeDelta = new Vector2(Screen.width / 10, Screen.width / 16);
        timerText.fontSize = (int)(Screen.height * 0.04f);
    }

    public void startTimer(float arg_time)
    {
        slider.value = 0;
        this.gameObject.SetActive(true);
        slider.maxValue = arg_time;
    }

    public void UpdateTimer(float currentTime, float time, int increments)
    {
        timerTextObject.SetActive(true);
        timerText.text = "Sea bed: " + (Mathf.Round(time - currentTime) * 10).ToString() + "m";
        slider.value += (time / increments);
    }

}
