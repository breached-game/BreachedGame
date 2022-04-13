using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicManagerScript : MonoBehaviour
{
    public GameObject commandObject;
    private CommandManager commandManager;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        // removed hello testing call as gave error abort on stack trace. 
        //Debug.Log("About to call hello function");
        //VoiceWrapper.Hello();
        //Debug.Log("finsihed calling hello function");
        commandManager = commandObject.GetComponent<CommandManager>();
        Debug.Log("About to call start function");
        VoiceWrapper.start();
        Debug.Log("finsihed calling start function");
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Message printed when microphone has been recieved
    void MicRecieved()
    {
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

    //Peer lost connection 
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
