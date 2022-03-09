using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Message printed when mirphone has been recieved
    void MicRecieved() {
        print("Microphone recieved");
    }

    //Message printed when mirphone has been rejected
    void MicRejeted()
    {
        print("Microphone rejected");
    }

    //Message printed when mirphone has been Accepted
    void MicAccepted()
    {
        print("Microphone accepted");
    }
}
