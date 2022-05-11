using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteButtonManager : MonoBehaviour
{
    /*
        HANDLES THE SIZING AND STATE CHANGES OF THE MUTE BUTTON

        Contributors: Sam Barnes-Thornton and Andrew Morgan
    */
    private RectTransform rectTransform;
    public Sprite muted;
    public Sprite unmuted;
    private Image actualImage;
    private int size = Screen.width / 25;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        actualImage = gameObject.GetComponent<Image>();
        // Checks whether previous player preference was muted
        // and changes image accordingly
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
        size = Screen.width / 25;
        rectTransform.sizeDelta = new Vector2(size, size);
        rectTransform.localPosition = new Vector3(Screen.width/2 - size/2, Screen.height/2 - size/2);
        // Allows user to press M to mute/unmute
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
            // Calls jslib function to unmute webrtc
            VoiceWrapper.muteMic();
        }
        else
        {
            PlayerPrefs.SetInt("Mute", 1);
            print("muted");
            actualImage.sprite = muted;
            // Calls jslib function to mute webrtc
            VoiceWrapper.muteMic();
        }
    }
}
