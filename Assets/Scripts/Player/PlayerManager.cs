using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    public GameObject PlayerModel;
    public GameObject FirstPersonCamera;
    public GameObject PlayerCurrentlyHolding;

    private NetworkIdentity identity;

    private CharacterController _controller;

    public GameObject objectPlayerHas = null;
    public GameObject torch;

    public bool disableInteractionsForMinigame = false;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        identity = GetComponent<NetworkIdentity>();
        Cursor.lockState = CursorLockMode.Locked;

        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);

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
    #region Player Sync
    [Command]
    public void CmdAssignAurthority(GameObject wantsAurthority)
    {
        GetComponent<NetworkIdentity>().connectionToClient.clientOwnedObjects.Add(wantsAurthority.GetComponent<NetworkIdentity>());
        wantsAurthority.gameObject.GetComponent<NetworkIdentity>().AssignClientAuthority(GetComponent<NetworkIdentity>().connectionToClient);
        print("Player has given " + wantsAurthority.transform.name + " Aurthority");


        /*var networkIdentity = wantsAurthority.gameObject.GetComponent<NetworkIdentity>();
        networkIdentity.AssignClientAuthority(identity.connectionToClient);*/

    }
    [Command]
    public void CmdRemoveAurthority(GameObject wantsRemovedAurthority)
    {
        wantsRemovedAurthority.GetComponent<NetworkIdentity>().RemoveClientAuthority();
        print("Player has removed " + wantsRemovedAurthority.transform.name + " Aurthority");
        /* var networkIdentity = wantsRemovedAurthority.gameObject.GetComponent<NetworkIdentity>();
         networkIdentity.AssignClientAuthority(identity.connectionToClient);
        */
    }

    public void CallCmdPickupObject(GameObject objectBeingPickedUp)
    {
        CmdPickUpObject(objectBeingPickedUp);
    }
   

    [Command]
    public void CmdPickUpObject(GameObject objectBeingPickedUp)
    {
        //Check that we still can pick up the object
        if (objectBeingPickedUp.activeInHierarchy)
        {
            UpdatePickUpObjects(this.gameObject, objectBeingPickedUp);
            GameObject player = this.gameObject;
            //Runs for everyone so all instances say that this player has this object 
            PlayerManager playerManager = player.GetComponent<PlayerManager>();
            playerManager.objectPlayerHas = objectBeingPickedUp;
            playerManager.updateItemText();
            objectBeingPickedUp.SetActive(false);
            playerManager.VisualEffectOfPlayerPickingUpItem(objectBeingPickedUp);
        }
        else print("Player tried to pick up none active gameobject");
    }
    [ClientRpc]
    void UpdatePickUpObjects(GameObject player, GameObject objectBeingPickedUp)
    {
        //Runs for everyone so all instances say that this player has this object 
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        playerManager.objectPlayerHas = objectBeingPickedUp;
        if (playerManager.identity.isLocalPlayer)
        {
            playerManager.updateItemText();
        }
        objectBeingPickedUp.SetActive(false);
        playerManager.VisualEffectOfPlayerPickingUpItem(objectBeingPickedUp);
    }
    [Command]
    public void CmdDropItem()
    {
        UpdateDropItem(identity);
        GameObject player = identity.gameObject;
        PlayerManager playerManager = identity.gameObject.GetComponent<PlayerManager>();
        GameObject droppedItem = playerManager.objectPlayerHas;
        //Drop current item
        droppedItem.SetActive(true);
        droppedItem.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 1, player.transform.position.z); //Currrently -1 for player height, objects will float
        if (playerManager.objectPlayerHas.transform.name == "Torch")//Torch is different to generic object.
        {
            playerManager.torch.SetActive(false);
        }
        playerManager.objectPlayerHas = null;
        //Update the item text
        playerManager.updateItemText();
        //Visual Effect
        playerManager.VisualEffectOfPlayerDroppingItem();
    }
    [ClientRpc]
    void UpdateDropItem(NetworkIdentity playerID)
    {
        PlayerManager playerManager = playerID.gameObject.GetComponent<PlayerManager>();
        GameObject player = playerID.gameObject;
        GameObject droppedItem = playerManager.objectPlayerHas;
        //Drop current item
        droppedItem.SetActive(true);
        droppedItem.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 1, player.transform.position.z); //Currrently -1 for player height, objects will float
        if (playerManager.objectPlayerHas.transform.name == "Torch")//Torch is different to generic object.
        {
            playerManager.torch.SetActive(false);
        }
        playerManager.objectPlayerHas = null;
        //Update the item text
        playerManager.updateItemText();
        //Visual Effect
        playerManager.VisualEffectOfPlayerDroppingItem();
    }
    #endregion

    #region Visual Effects
    void VisualEffectOfPlayerPickingUpItem(GameObject objectPickedUp)
    {
        if (objectPickedUp.name != "Torch")
        {
            Animator playerAni = PlayerModel.GetComponent<Animator>();
            playerAni.Play("Walk_Carry");
            playerAni.SetBool("Holding", true);
            for (int i = 0; i < PlayerCurrentlyHolding.transform.childCount; i++)
            {
                if (PlayerCurrentlyHolding.transform.GetChild(i).gameObject.name == objectPickedUp.GetComponent<ItemPickUp>().itemBeingHeld.name)
                {
                    PlayerCurrentlyHolding.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
    }
    public void VisualEffectOfPlayerDroppingItem()
    {
        //Stop animation and empty hands
        PlayerModel.GetComponent<Animator>().SetBool("Holding", false);
        for (int i = 0; i < PlayerCurrentlyHolding.transform.childCount; i++)
        {
            PlayerCurrentlyHolding.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    #endregion

    public float Speed = 4.0f;
    public float SprintSpeed = 6.0f;
    public float smooth = 5f;
    private float translation;
    private float straffe;


    void Update()
    {
        if (identity.isLocalPlayer)
        {
            // Input.GetAxis() is used to get the user's input
            // You can furthor set it on Unity. (Edit, Project Settings, Input)
            Vector3 move = new Vector3(translation, 0, straffe);

            if(Input.GetAxisRaw("Sprint") != 0)
            {
                translation = Input.GetAxis("Vertical") * SprintSpeed * Time.deltaTime;
                straffe = Input.GetAxis("Horizontal") * SprintSpeed * Time.deltaTime;
                PlayerModel.GetComponent<Animator>().SetBool("Sprint", true);
            }
            else
            {
                translation = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
                straffe = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
                PlayerModel.GetComponent<Animator>().SetBool("Sprint", false);
            }
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
                CmdDropItem();
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

            //Animation Control
            if(move == new Vector3(0, 0, 0))
            {
                PlayerModel.GetComponent<Animator>().SetBool("Moving", false);
            } else PlayerModel.GetComponent<Animator>().SetBool("Moving", true);
        }
    }

    public void updateItemText()
    {
        //TERRIBLE PRACTICE
        GameObject UI = GameObject.Find("Canvas/PlayerUI");
        PlayerUIManager UIManager;
        // Andrew's fault
        if (UI != null)
        {
            UIManager = UI.GetComponent<PlayerUIManager>();
            if (objectPlayerHas != null) UIManager.UpdatePlayerHolding(objectPlayerHas.transform.name);
            else UIManager.UpdatePlayerHolding("");
        }
    }
}
