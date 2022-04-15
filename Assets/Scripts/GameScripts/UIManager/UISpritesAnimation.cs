using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class UISpritesAnimation : MonoBehaviour
{
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
        for (int i = 0; i < sprites.Length; i++)
        {
            //i need help here i have no idea why image is null sometimes -Andy
            if (image != null) image.sprite = sprites[i];
            yield return new WaitForSeconds(duration);
        }
        gameObject.SetActive(false);
    } 
}


