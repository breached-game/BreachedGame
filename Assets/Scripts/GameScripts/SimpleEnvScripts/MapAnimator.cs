using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAnimator : MonoBehaviour
{
    /*
        SCRIPT FOR ANIMATING THE ORIGINAL 2D MAP
        NOT IN THE CURRENT VERSION OF THE GAME 
        BUT INCLUDED TO SHOW ITERATIONS

        Contributors: Sam Barnes-Thornton
    */

    // Map image with flashes
    public Sprite flashMap;
    // Plain map image
    public Sprite map;
    // Flash interval
    private float interval = 0.5f;
    private bool flash = false;
    private SpriteRenderer mapRenderer;
    // Start is called before the first frame update
    void Start()
    {
        // Setting variables to Unity references for use later on
        mapRenderer = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(MapFlash());
    }

    // Coroutine to manage the map flashing
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
