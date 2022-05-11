using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndStartGameCutScence : MonoBehaviour
{
    /*
        SIMPLE SCRIPT FOR RUNNING THE CUT SCENE

        Contributors: Andrew Morgan
    */
    void Start()
    {
        StartCoroutine(cutscence());
    }

    IEnumerator cutscence()
    {
        yield return new WaitForSeconds(7f);
    }
}
