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
    // Start is called before the first frame update
    void Awake()
    {
        slider = this.gameObject.GetComponent<Slider>();
        this.gameObject.SetActive(false);
        timerTextObject.SetActive(false);
        timerText = timerTextObject.GetComponent<Text>();
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
