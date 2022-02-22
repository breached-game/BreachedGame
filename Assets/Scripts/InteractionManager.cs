using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InteractionManager : NetworkBehaviour
{
    //We manage all interaction here
    [SyncVar]
    public bool available = true;

    public List<BoxCollider> disableWhenOpen;

    enum Type
    {
        MiniGame,
        Button,
        PickUp,
        DropOff,
        ColourButton,
        StartGameButton, 
    };
    [SerializeField] Type typeMenu;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && available)
        {
            if (other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                //Okay so GetKeyDown actually sucks
                //We check to see if the player is already interacting via the animator
                Animator playerAni = other.gameObject.GetComponent<PlayerManager>().PlayerModel.GetComponent<Animator>();
                if (Input.GetAxis("Interact") == 1 && !playerAni.GetCurrentAnimatorStateInfo(0).IsName("Interact") )
                {
                    playerAni.Play("Interact");
                    RaycastHit hit;
                    int maxDistance = 10000;

                    Transform cameraTransform = other.gameObject.GetComponent<PlayerManager>().FirstPersonCamera.transform;
                    if (Physics.Raycast(cameraTransform.transform.position, cameraTransform.forward, out hit, maxDistance))
                    {
                        //print(hit.transform.name);
                        InteractionManager interactable = hit.collider.GetComponent<InteractionManager>();
                        if (interactable != null && (hit.collider.gameObject == gameObject || hit.collider.gameObject == gameObject.transform.GetChild(0).gameObject))
                        {
                            //Display interaction available
                            //UIManager.ShowInteractionText(true);
                            //Accept input and trigger event
                            //Debug.Log("E pressed");
                            if (typeMenu == Type.MiniGame)
                            {
                                //Change to minigame scene
                                Debug.Log("Interacting with minigame");
                                // need to build server for this


                            }
                            if (typeMenu == Type.ColourButton)
                            {
                                if (transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                                {
                                    GetComponent<ColourMiniGameButton>().buttonPressed();
                                    transform.GetChild(0).GetComponent<Animator>().Play("Click");
                                }
                            }
                            if (typeMenu == Type.StartGameButton)
                            {
                                if (transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                                {
                                    GetComponent<StartGameButton>().startGame();
                                    transform.GetChild(0).GetComponent<Animator>().Play("Click");
                                }
                            }

                            if (typeMenu == Type.PickUp)
                            {
                                if (other.GetComponent<PlayerManager>().objectPlayerHas == null)
                                {
                                    //if the player is not carrying anything
                                    Debug.Log("Picked up" + this.gameObject.name);
                                    other.GetComponent<PlayerManager>().objectPlayerHas = this.gameObject;
                                    this.gameObject.SetActive(false);
                                }
                                else
                                {
                                    print("Player is already carrying an item = " + other.GetComponent<PlayerManager>().objectPlayerHas);
                                }
                            }

                            if (typeMenu == Type.DropOff)
                            {
                                if (other.GetComponent<PlayerManager>().objectPlayerHas.name == "WaterPumpItem")
                                {
                                    other.GetComponent<PlayerManager>().objectPlayerHas.GetComponent<WaterManager>().OutflowWater();
                                }
                                print("Try to drop off");
                                GetComponent<DropOff>().droppingOffItem(other.GetComponent<PlayerManager>().objectPlayerHas, other.GetComponent<PlayerManager>(), gameObject.transform.position);
                            }
                        }
                    }
                }
            } //else UIManager.ShowInteractionText(false);
        } //else UIManager.ShowInteractionText(false);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && available)
        {
             //other.gameObject is the local player
             //Display interaction not available
             //UIManager.ShowInteractionText(false);
            if(other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                TakestAuthorityAway();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        print("entered on trigger enter");       
        if (other.gameObject.tag == "Player" && available)
        {
            if (other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                GiveAuthority(other.gameObject);
            }
        }
    }
    [Command]
    void TakestAuthorityAway()
    {
        print("doin your mum");
        //GetComponent<NetworkIdentity>().RemoveClientAuthority();
    }
    [Command]
    void GiveAuthority(GameObject player)
    {
        print("doin doin your mum");
        //GetComponent<NetworkIdentity>().AssignClientAuthority(player.GetComponent<NetworkIdentity>().connectionToServer);
    }
}
