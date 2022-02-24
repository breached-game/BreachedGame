using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reveal : MonoBehaviour
{
    public Material material;
    public Light lightSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        material.SetVector("_LightPosition",lightSource.transform.position);
        material.SetVector("_LightDirection", -lightSource.transform.forward);
        material.SetFloat("_LightAngle", lightSource.spotAngle);
        material.SetFloat("_LightRange", lightSource.range);
    }
}
