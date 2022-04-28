using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMuteAtStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Mute", 0);
    }
}
