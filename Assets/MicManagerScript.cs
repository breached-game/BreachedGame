using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
     void Start()
    {
        Debug.Log("About to call hello function");
        VoiceWrapper.Hello();
        Debug.Log("finsihed calling hello function");
        Debug.Log("About to call start function");
        VoiceWrapper.start();
        Debug.Log("finsihed calling start function");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Message printed when microphone has been recieved
    void MicRecieved() {
        print("Microphone permission has been granted");
    }

    //Message printed when microphone has been rejected
    void MicRejected()
    {
        print("Microphone permission has been declined");
    }

    //Message printed when microphone has been Accepted
    void MicAccepted()
    {
        print("Microphone permission has been granted");
    }
}
