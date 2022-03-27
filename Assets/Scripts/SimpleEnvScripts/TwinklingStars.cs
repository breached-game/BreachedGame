using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinklingStars : MonoBehaviour
{
    public Material offStar;
    private Material onStar;
    public float duration;

    // Start is called before the first frame update
    void Start()
    {
        onStar = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material;
        StartCoroutine(play());
    }
    IEnumerator play()
    {
        while (true)
        {
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
