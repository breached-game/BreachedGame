using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class UISpritesAnimation : MonoBehaviour
{
    /*
        HANDLES THE ANIMATION OF THE OBJECTIVE COMPLETE GIF

        Contributors: Andrew Morgan
    */
    public float duration;

    [SerializeField] private Sprite[] sprites;

    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }
    public void Play()
    {
        gameObject.SetActive(true);
        StartCoroutine(playGif());
    }
    IEnumerator playGif()
    {
        if (sprites != null)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                if (image != null) image.sprite = sprites[i];
                yield return new WaitForSeconds(duration);
            }
            gameObject.SetActive(false);
        }
    } 
}


