using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameCameraScript : MonoBehaviour
{
    /*
        SCRIPT FOR MANAGING THE MINIMAP CAMERA
        THE MAP IS ACHIEVED BY POSITIONING A CAMERA ABOVE THE MAP
        WHICH THEN RENDERS IT'S VIEW ONTO THE UI

        Contributors: Seth Holdcroft
    */

    // Where the first floor is
    private Vector3 firstFloorTransform = new Vector3(5.4f, 300, -17.4f);
    private float firstFloorFOV = 13.6f;

    // Where the second floor is
    private Vector3 secondFloorTransform = new Vector3(5.4f, 300, -0.96f);
    private float secondFloorFOV = 15.8f;

    private Camera cameraComponet; 



    void Awake()
    { 
        DontDestroyOnLoad(this.gameObject);
        cameraComponet = gameObject.GetComponent<Camera>();

    }

    // Changes camera culling mask to only render first floor
    public void TransformToFirstFloorView()
    {
        gameObject.transform.position = firstFloorTransform;
        cameraComponet.fieldOfView = firstFloorFOV;
        cameraComponet.cullingMask = 256;
    }

    // Changes camera culling mask to only render second floor
    public void TransformToSecondFloorView()
    {
        gameObject.transform.position = secondFloorTransform;
        cameraComponet.fieldOfView = secondFloorFOV;
        cameraComponet.cullingMask = 512;
    }

  

}
