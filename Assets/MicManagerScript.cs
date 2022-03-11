using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        VoiceWrapper.Hello();
        VoiceWrapper.RequestMic();
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
    void MicRejeted()
    {
        print("Microphone permission has been declined");
    }

    //Message printed when microphone has been Accepted
    void MicAccepted()
    {
        print("Microphone accepted");
    }
}
