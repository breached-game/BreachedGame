using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
THIS CLASS HANDLES COMMUNICATION BETWEEN JAVASCRIPT AND UNITY TO INITIALISE VOICE COMMUNICATION.
Contributors: Daniel Savidge and Luke Benson 
*/


public class MicManagerScript : MonoBehaviour
{
    public GameObject commandObject;
    private CommandManager commandManager;
    //Stop object getting destroyed on scene change
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    //Gets command line object and starts voice communication process
    void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            commandManager = commandObject.GetComponent<CommandManager>();
            VoiceWrapper.start();
        }
    }

    void Update()
    {

    }

    //Message printed when microphone has been recieved
    void MicRecieved()
    {
        print("Microphone permission has been recieved");
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

    //Outputting connection status to captain command line, called from JavaScript
    void PrintMsgCaptain(string state)
    {
        
        if (state == "failed" || state == "closed" || state == "disconnected")
        {
            commandManager.QueueMessage("You have lost voice connection to a Teammate");
        } else if (state == "connected") {
            commandManager.QueueMessage("New connection to a Teammate");
        }
    }
}
