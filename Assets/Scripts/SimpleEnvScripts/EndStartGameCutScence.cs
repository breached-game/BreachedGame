using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndStartGameCutScence : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(cutscence());
    }

    IEnumerator cutscence()
    {
        yield return new WaitForSeconds(7f);
        print("Here we load the scence");
    }
}
