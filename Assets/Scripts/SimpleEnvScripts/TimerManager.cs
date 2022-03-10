using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    Slider slider;
    int increments = 1200;
    float time;
    public GameObject timerTextObject;
    private Text timerText;
    public GameObject failScreen;
    // Start is called before the first frame update
    void Start()
    {
        slider = this.gameObject.GetComponent<Slider>();
        this.gameObject.SetActive(false);
        timerTextObject.SetActive(false);
        timerText = timerTextObject.GetComponent<Text>();
    }

    public void startTimer(float arg_time)
    {
        time = arg_time;
        slider.value = 0;
        this.gameObject.SetActive(true);
        slider.
        StartCoroutine(timer());
    }

    IEnumerator timer()
    {
        float currentTime = 0f;
        slider.maxValue = time;
        while (currentTime < time)
        {
            if (time - currentTime < 60)
            {
                timerTextObject.SetActive(true);
                timerText.text = (Mathf.Round(time-currentTime)).ToString();
            }
            slider.value += (time / increments);
            currentTime += (time / increments);
            yield return new WaitForSeconds(time/increments);
        }
        failScreen.SetActive(true);
    }

}
