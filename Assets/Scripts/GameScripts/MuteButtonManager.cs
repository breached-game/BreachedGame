using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteButtonManager : MonoBehaviour
{
    private RectTransform rectTransform;
    public Sprite muted;
    public Sprite unmuted;
    private Image actualImage;
    private int size = Screen.width / 15;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        actualImage = gameObject.GetComponent<Image>();
        if(PlayerPrefs.GetInt("Mute") == 1)
        {
            actualImage.sprite = muted;
        }
        else
        {
            actualImage.sprite = unmuted;
        }
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.sizeDelta = new Vector2(size, size);
        rectTransform.localPosition = new Vector3(Screen.width/2 - size/2, Screen.height/2 - size/2);
        if (Input.GetKeyDown(KeyCode.M))
        {
            ChangeState();
        }
    }

    public void ChangeState()
    {
        if (actualImage.sprite == muted)
        {
            PlayerPrefs.SetInt("Mute", 0);
            print("unmuted");
            actualImage.sprite = unmuted;
            VoiceWrapper.muteMic();
        }
        else
        {
            PlayerPrefs.SetInt("Mute", 1);
            print("muted");
            actualImage.sprite = muted;
            VoiceWrapper.muteMic();
        }
    }
}
