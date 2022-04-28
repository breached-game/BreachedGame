using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlRodUISetup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RectTransform ControlRodUI = this.gameObject.GetComponent<RectTransform>();
        //ControlRodUI.sizeDelta = new Vector2(mainMenuTransform.rect.width * 3 / 4, mainMenuTransform.rect.height / 8);
        ControlRodUI.localPosition = new Vector3(Screen.height / 2, Screen.width/2, 0);
    }
}
