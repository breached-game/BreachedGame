using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAnimator : MonoBehaviour
{
    public Sprite flashMap;
    public Sprite map;
    private float interval = 0.5f;
    private bool flash = false;
    private SpriteRenderer mapRenderer;
    // Start is called before the first frame update
    void Start()
    {
        mapRenderer = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(MapFlash());
    }

    IEnumerator MapFlash()
    {
        while (true)
        {
            if (flash)
            {
                mapRenderer.sprite = flashMap;
                flash = false;
            }
            else
            {
                mapRenderer.sprite = map;
                flash = true;
            }
            yield return new WaitForSeconds(interval);
        }
    }
}
