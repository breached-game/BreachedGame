using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAnimator : MonoBehaviour
{
    public Sprite flashMap;
    public Sprite map;
    private float interval = 0.5f;
    private bool flash = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MapFlash());
    }

    IEnumerator MapFlash()
    {
        while (true)
        {
            if (flash)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = flashMap;
                flash = false;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = map;
                flash = true;
            }
            yield return new WaitForSeconds(interval);
        }
    }
}
