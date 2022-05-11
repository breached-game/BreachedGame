using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinklingStars : MonoBehaviour
{
    /*
        AESTHETICS SCRIPT FOR ANIMATING THE TWINKLING STARS SEEN
        IN THE VICTORY SCREEN

        Contributors: Andrew Morgan
    */
    public Material offStar;
    private Material onStar;
    public float duration;

    // Start is called before the first frame update
    void Start()
    {
        // Sets material of twinkling star to be default
        onStar = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material;
        StartCoroutine(play());
    }

    // Coroutine used to play the twinkling effect
    IEnumerator play()
    {
        // Loops infinitely until the scene is changed
        while (true)
        {
            // Uses different modulo values to have a different subset of stars twinkling in each time period
            for(int i = 0; i < transform.childCount; i++)
            {
                if((i % 3) == 0)
                {
                    transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = offStar;
                }
                else transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = onStar;
            }
            yield return new WaitForSeconds(duration);
            for (int i = 0; i < transform.childCount; i++)
            {
                if ((i % 2) == 0)
                {
                    transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = offStar;
                }
                else transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = onStar;
            }
            yield return new WaitForSeconds(duration);
            for (int i = 0; i < transform.childCount; i++)
            {
                if ((i % 3) == 1)
                {
                    transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = offStar;
                }
                else transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = onStar;
            }
            yield return new WaitForSeconds(duration);
        }
    }
}
