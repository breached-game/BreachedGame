using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

using System.Collections;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    public GameObject PlayerModel;
    public GameObject FirstPersonCamera;

    public NetworkIdentity identity;

    private CharacterController _controller;

    public GameObject objectPlayerHas = null;
    public GameObject torch;

    public readonly SyncDictionaryStringString currentObjectives = new SyncDictionaryStringString();
    public readonly SyncDictionaryStringString doneObjectives = new SyncDictionaryStringString();

    GameObject playerUI;

    void Awake()
    {
        playerUI = GameObject.Find("/Canvas/PlayerUI");
        if (playerUI == null)
        {
            Debug.LogError("No playerUI - buggy code");
        }
        _controller = GetComponent<CharacterController>();
        identity = GetComponent<NetworkIdentity>();
        Cursor.lockState = CursorLockMode.Locked;

        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);

        //Start Animation Control
        StartCoroutine(animationControll());

    }

    public override void OnStartClient()
    {
        currentObjectives.Callback += OnObjectiveChange;
        doneObjectives.Callback += OnObjectiveChange;
    }
    public void OnObjectiveChange(SyncDictionaryStringString.Operation op, string key, string value)
    {
        Debug.Log("Callback");
        Debug.Log("Current Objectives: " + currentObjectives.Count);
        Debug.Log("Done Objectives: " + doneObjectives.Count);
        List<string> objectiveNames = new List<string>();
        List<string> objectiveDescriptions = new List<string>();
        List<string> completedObjectiveNames = new List<string>();
        List<string> completedObjectiveDescriptions = new List<string>();
        foreach (var o in currentObjectives)
        {
            objectiveNames.Add(o.Key);
            objectiveDescriptions.Add(o.Value);
        }
        foreach (var o in doneObjectives)
        {
            completedObjectiveNames.Add(o.Key);
            completedObjectiveDescriptions.Add(o.Value);
        }
        switch (op)
        {
            case SyncIDictionary<string, string>.Operation.OP_ADD:
                if (doneObjectives.Count + currentObjectives.Count == 3)
                {
                    playerUI.GetComponent<PlayerUIManager>().UpdateObjectiveUI(objectiveNames.ToArray(), objectiveDescriptions.ToArray(), completedObjectiveNames.ToArray(), completedObjectiveDescriptions.ToArray());

                }
                break;
            case SyncIDictionary<string, string>.Operation.OP_CLEAR:
                break;
            case SyncIDictionary<string, string>.Operation.OP_REMOVE:
                break;
            case SyncIDictionary<string, string>.Operation.OP_SET:
                break;
            default:
                break;
        }
    }

    [Command]
    public void CmdUpdateObjectiveList(string[] objectiveNames, string[] objectiveDescriptions, string[] completedObjectiveNames, string[] completedObjectiveDescriptions)
    {
        Debug.Log("Command called");
        Debug.Log("Done:" + completedObjectiveNames.Length);
        currentObjectives.Clear();
        doneObjectives.Clear();
        for (int i = 0; i < objectiveNames.Length; i++)
        {
            currentObjectives.Add(objectiveNames[i], objectiveDescriptions[i]);
        }
        for (int i = 0; i < completedObjectiveNames.Length; i++)
        {
            doneObjectives.Add(completedObjectiveNames[i], completedObjectiveDescriptions[i]);
        }
    }

    public void TurnOnAudio()
    {
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (identity.isLocalPlayer)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
            FirstPersonCamera.GetComponent<AudioListener>().enabled = true;
        }
    }


    public void TurnOffAudio()
    {
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (identity.isLocalPlayer)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
            FirstPersonCamera.GetComponent<AudioListener>().enabled = false;
        }
    }

    #region Movement
    public float Speed = 4.0f;
    public float smooth = 5f;
    private float translation;
    private float straffe;


    void Update()
    {
        if (identity.isLocalPlayer)
        {
            // Input.GetAxis() is used to get the user's input
            // You can furthor set it on Unity. (Edit, Project Settings, Input)
            translation = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
            straffe = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
            Vector3 move = new Vector3(translation, 0, straffe);

            _controller.Move(translation *  transform.forward);
            _controller.Move(straffe *  transform.right);

            if (Input.GetKeyDown("escape"))
            {
                // turn on the cursor
                Cursor.lockState = CursorLockMode.None;
            }
            
            if (!_controller.isGrounded)
            {
                _controller.Move(new Vector3(0, -10, 0) * Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.G) && objectPlayerHas != null)
            {
                //Drop current item

                objectPlayerHas.transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z); //Currrently -1 for player height, objects will float
                objectPlayerHas.SetActive(true);
                if (objectPlayerHas.transform.name == "Torch")//Torch is different to generic object.
                {
                    torch.SetActive(false);
                }
                objectPlayerHas = null;
                //Update the item text
                updateItemText();
            }

            //check if we have torch 
            if (objectPlayerHas != null)
            {
                if (objectPlayerHas.transform.name == "Torch")
                {
                    torch.SetActive(true);
                }
                //Update the item text
                updateItemText();
            }
        }
    }

    public void updateItemText()
    {
        //TERRIBLE PRACTICE
        GameObject UI = GameObject.Find("Canvas/PlayerUI");
        if (UI == null) Debug.LogError("Player Script cannot access player UI --Andrew's fault");
        if (objectPlayerHas != null) UI.GetComponent<PlayerUIManager>().UpdatePlayerHolding(objectPlayerHas.transform.name);
        else UI.GetComponent<PlayerUIManager>().UpdatePlayerHolding("");
    }
    IEnumerator animationControll()
    {
        while (true)
        {
            //Animation control
            Vector3 prevPos = transform.position;
            yield return new WaitForSeconds(0.5f);
            Vector3 actualPos = transform.position;

            if (prevPos == actualPos) PlayerModel.GetComponent<Animator>().SetBool("Moving", false);
            if (prevPos != actualPos) PlayerModel.GetComponent<Animator>().SetBool("Moving", true);
        }
    }
    #endregion
}
