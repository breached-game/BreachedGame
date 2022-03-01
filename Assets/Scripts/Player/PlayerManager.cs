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

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        identity = GetComponent<NetworkIdentity>();
        Cursor.lockState = CursorLockMode.Locked;

        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);

        //Start Animation Control
        StartCoroutine(animationControll());

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
   

    [Command]
    public void CmdPickUpObject(GameObject objectBeingPickedUp)
    {
        //Check that we still can pick up the object
        if (objectBeingPickedUp.activeInHierarchy)
        {
            UpdatePickUpObjects(this.gameObject, objectBeingPickedUp);
        }
        else print("Player tried to pick up none active gameobject");
    }
    [ClientRpc]
    void UpdatePickUpObjects(GameObject player, GameObject objectBeingPickedUp)
    {
        //Runs for everyone so all instances say that this player has this object 
        player.GetComponent<PlayerManager>().objectPlayerHas = objectBeingPickedUp;
        player.GetComponent<PlayerManager>().updateItemText();
        objectBeingPickedUp.SetActive(false);
        player.GetComponent<PlayerManager>().VisualEffectOfPlayerPickingUpItem(objectBeingPickedUp);
    }
    [Command]
    public void CmdDropItem()
    {
        UpdateDropItem(this.gameObject);
    }
    //Code runs on all clients so we need to be accurate about which player this is happening for
    [ClientRpc]
    void UpdateDropItem(GameObject player)
    {
        GameObject droppedItem = player.GetComponent<PlayerManager>().objectPlayerHas;
        //Drop current item
        droppedItem.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 1, player.transform.position.z); //Currrently -1 for player height, objects will float
        droppedItem.SetActive(true);
        if (player.GetComponent<PlayerManager>().objectPlayerHas.transform.name == "Torch")//Torch is different to generic object.
        {
            player.GetComponent<PlayerManager>().torch.SetActive(false);
        }
        player.GetComponent<PlayerManager>().objectPlayerHas = null;
        //Update the item text
        player.GetComponent<PlayerManager>().updateItemText();
        //Visual Effect
        player.GetComponent<PlayerManager>().VisualEffectOfPlayerDroppingItem();
    }
    #endregion

    #region Visual Effects
    void VisualEffectOfPlayerPickingUpItem(GameObject objectPickedUp)
    {
        PlayerModel.GetComponent<Animator>().Play("Walk_Carry");
        PlayerModel.GetComponent<Animator>().SetBool("Holding", true);
        for(int i = 0; i < PlayerCurrentlyHolding.transform.childCount; i++)
        {
            if (PlayerCurrentlyHolding.transform.GetChild(i).gameObject.name == objectPickedUp.GetComponent<ItemPickUp>().itemBeingHeld.name)
            {
                PlayerCurrentlyHolding.transform.GetChild(i).gameObject.SetActive(true);
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
}
