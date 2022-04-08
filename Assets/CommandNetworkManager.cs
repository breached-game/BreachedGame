using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandNetworkManager : MonoBehaviour
{
    public GameObject commandObject;
    private CommandManager commandManager;
    // Start is called before the first frame update
    void Start()
    {
        commandManager = commandObject.GetComponent<CommandManager>();
    }

    public void QueueNetworkMessage(string msg, bool captain)
    {
        print("Msg");
        commandManager.QueueMessage(msg, captain);
    }
}
