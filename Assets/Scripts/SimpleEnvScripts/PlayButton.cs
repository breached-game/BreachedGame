using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public GameObject Canvas;
    public Material MouseOnColour;
    private Material preMat;

    private void Start()
    {
        preMat = GetComponent<MeshRenderer>().material;
    }
    private void OnMouseEnter()
    {
            GetComponent<MeshRenderer>().material = MouseOnColour;
    }
    private void OnMouseExit()
    {
            GetComponent<MeshRenderer>().material = preMat;
    }
    private void OnMouseDown()
    {
        Canvas.GetComponent<LauncherUI>().StartButtonClient();
    }
}
