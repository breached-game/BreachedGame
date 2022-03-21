using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleControlRodUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int a = 0; a < transform.childCount; a++) {
            for (int b = 0; b < transform.GetChild(a).transform.childCount; b++)
            {
                RectTransform Button = transform.GetChild(a).transform.GetChild(b).GetComponent<RectTransform>();
                Button.sizeDelta = new Vector2(Screen.width / 4, Screen.height / 16);
                Button.localPosition = new Vector3(Screen.width/32, -1 * Screen.height * (b*2+1) / 32, 0);
            }
        }
    }
}
