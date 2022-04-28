using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameCameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 firstFloorTransform = new Vector3(5.4f, 300, -17.4f);
    private float firstFloorFOV = 13.6f;

    private Vector3 secondFloorTransform = new Vector3(5.4f, 300, -0.96f);
    private float secondFloorFOV = 15.8f;

    private Camera cameraComponet; 



    void Awake()
    { 
        DontDestroyOnLoad(this.gameObject);
        cameraComponet = gameObject.GetComponent<Camera>();

    }
    public void TransformToFirstFloorView()
    {

        gameObject.transform.position = firstFloorTransform;
        cameraComponet.fieldOfView = firstFloorFOV;
        cameraComponet.cullingMask = 256;
    }

    public void TransformToSecondFloorView()
    {
        gameObject.transform.position = secondFloorTransform;
        cameraComponet.fieldOfView = secondFloorFOV;
        cameraComponet.cullingMask = 512;
    }

  

}
