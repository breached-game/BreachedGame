using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    /*
        SCRIPT FOR MANAGING ALL PLAYER INTERACTIONS AND MOVWEMENT WITHIN THE GAME ENVIRONMENT

        Contributors: Andrew Morgan, Sam Barnes-Thornton and Seth Holdcroft
    */
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    public GameObject PlayerModel;
    public GameObject FirstPersonCamera;
    public GameObject PlayerCurrentlyHolding;

    private NetworkIdentity identity;

    private CharacterController _controller;

    public GameObject objectPlayerHas = null;
    public GameObject torch;

    // Used for when a player has temporarily moved to a different camera for a minigam
    public bool disableInteractionsForMinigame = false;

    public float defaultSpeed;
    public float defaultSprintSpeed;
    public bool inWater = false;
    public bool soundPlaying = false;

    // Audio source for when the player is walking through water
    private AudioSource waterWalking;

    private GameObject minimapCamera;
    public GameObject minimapToken;
    private MinigameCameraScript minimapCameraController;


    private float switchFloorHeight = 3.0f;
    //

    void Awake()
    {
        waterWalking = GetComponent<AudioSource>();
        _controller = GetComponent<CharacterController>();
        identity = GetComponent<NetworkIdentity>();
        // Makes sure cursor is locked when a player first joins
        Cursor.lockState = CursorLockMode.Locked;

        // We flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
        defaultSpeed = Speed;
        defaultSprintSpeed = SprintSpeed;


        minimapToken = transform.GetChild(3).gameObject;
        if (minimapToken.name != "MinimapToken")
        {
            Debug.Log("Incorrect Minimap Token - Seths Fault");
        }

        minimapCamera = GameObject.FindGameObjectWithTag("MinimapCamera");
        minimapCameraController = minimapCamera.GetComponent<MinigameCameraScript>();
    }


    private void Start()
    {
        TurnOnAudio();
    }

    // Turns on local player's audio listener
    public void TurnOnAudio()
    {
        // Used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (identity.isLocalPlayer)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
            FirstPersonCamera.GetComponent<AudioListener>().enabled = true;
        }
    }


    // Used to turn off the local player's audio listener if another camera has to be used for some reason
    public void TurnOffAudio()
    {
        // Used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (identity.isLocalPlayer)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
            FirstPersonCamera.GetComponent<AudioListener>().enabled = false;
        }
    }
    #region Player Sync
    // Each local player has authority to call commands on the server
    // The commands called here are not in PlayerNetworkManage because
    // they directly relate to what the local player is holding
    public void CallCmdPickupObject(GameObject objectBeingPickedUp)
    {
        CmdPickUpObject(objectBeingPickedUp);
    }

    // Picks up an object if it is available and sets all necessary resulting parameters
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

    // Relays to all clients that the player has picked up an object
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
        // Visual effect is called using PlayerNetworkManager
        playerManager.VisualEffectOfPlayerPickingUpItem(objectBeingPickedUp);

    }
    
    // Drops an item if the player is holding one
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
        droppedItem.GetComponent<InteractionManager>().pickedUp = false;
    }

    // Relays to all clients that the player has dropped an item
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

        if (droppedItem.transform.name == "WaterPump")
        {
            droppedItem.GetComponent<WaterManager>().AddPump();
        }
        droppedItem.GetComponent<InteractionManager>().pickedUp = false;
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


    // Resets speed back to defaults when it has been changed (either for water or shaking)
    public void ResetSpeed()
    {
        Speed = defaultSpeed;
        SprintSpeed = defaultSprintSpeed;
    }

    void Update()
    {
        if (identity.isLocalPlayer)
        {
            // Input.GetAxis() is used to get the user's input
            // You can further set it on Unity. (Edit, Project Settings, Input)
            Vector3 move = new Vector3(translation, 0, straffe);

            // Checks whether the user is holding down shift (to sprint)
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
            
            // Makes sure player is on the ground (-10 is the approximated value for gravity)
            if (!_controller.isGrounded)
            {
                _controller.Move(new Vector3(0, -10, 0) * Time.deltaTime);
            }

            // Drops item if G is pressed
            if (Input.GetKeyDown(KeyCode.G) && objectPlayerHas != null)
            {
                CmdDropItem();
            }

            // Check if we have torch 
            if (objectPlayerHas != null)
            {
                if (objectPlayerHas.transform.name == "Torch")
                {
                    // Set light to be on
                    torch.SetActive(true);
                }
                // Update the item text
                updateItemText();
            }

            // Animation Control
            // Player is not moving:
            if (move == new Vector3(0, 0, 0))
            {
                // Moving animation is turned off
                PlayerModel.GetComponent<Animator>().SetBool("Moving", false);
                if (soundPlaying)
                {
                    // No water walking sound is played even if in water
                    waterWalking.Stop();
                    soundPlaying = false;
                }
            }
            // Player is moving
            else
            {
                // Moving animation turned on
                PlayerModel.GetComponent<Animator>().SetBool("Moving", true);
                // Turns on water walking sound if it is not playing and player is in water
                if (inWater && !soundPlaying)
                {
                    waterWalking.Play();
                    soundPlaying = true;
                }
                // Turns off water walking sound if player not in water
                else if(!inWater && soundPlaying)
                {
                    waterWalking.Stop();
                    soundPlaying = false;
                }
            }
        }
        tagMapMarkerToCurrentFloor();
    }


    // Moves player minimap marker to be on the right floor
    private void tagMapMarkerToCurrentFloor()
    {
        if (minimapToken.layer  == LayerMask.NameToLayer("FirstFloor"))
        {
            if (identity.transform.position.y > switchFloorHeight) // if player above the switch floor height
            {
                minimapToken.layer = LayerMask.NameToLayer("SecondFloor");
                if (identity.isLocalPlayer)
                {
                    minimapCameraController.TransformToSecondFloorView();
                }
            }
        }
        else
        {
            if (identity.transform.position.y < switchFloorHeight) // else on Second floor and if player below the switch floor height
            {
                minimapToken.layer = LayerMask.NameToLayer("FirstFloor");
                if (identity.isLocalPlayer)
                {
                    minimapCameraController.TransformToFirstFloorView();

                }
            }
        }

    }


    // Updates the item text for what the player is holding
    public void updateItemText()
    {
        GameObject UI = GameObject.Find("Canvas/PlayerUI");
        PlayerUIManager UIManager;
        if (UI != null)
        {
            UIManager = UI.GetComponent<PlayerUIManager>();
            if (objectPlayerHas != null) UIManager.UpdatePlayerHolding(objectPlayerHas.transform.name, objectPlayerHas.GetComponent<ItemPickUp>().desc);
            else UIManager.UpdatePlayerHolding("", "");
        }
    }
}
