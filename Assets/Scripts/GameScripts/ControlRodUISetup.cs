using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlRodUISetup : MonoBehaviour
{
    RectTransform ControlRodUI;
    // Start is called before the first frame update
    void Start()
    {
        ControlRodUI = this.gameObject.GetComponent<RectTransform>();
        //ControlRodUI.sizeDelta = new Vector2(mainMenuTransform.rect.width * 3 / 4, mainMenuTransform.rect.height / 8);
    }
    private void Update()
    {
        ControlRodUI.position = new Vector2(3 * Screen.width / 4, Screen.height / 2);
    }
}
