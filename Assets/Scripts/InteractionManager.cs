using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InteractionManager : NetworkBehaviour
{
    //We manage all interaction here
    [SyncVar]
    public bool available = true;

    public BoxCollider disableWhenOpen;

    enum Type
    {
        Door,
        MiniGame,
        Button,
        PickUp,
        DropOff,
        ColourButton,
    };
    [SerializeField] Type typeMenu;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && available)
        {
            if (other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                RaycastHit hit;
                int maxDistance = 100;
                if (Physics.Raycast(other.gameObject.GetComponent<PlayerManager>().FirstPersonCamera.transform.position, other.gameObject.GetComponent<PlayerManager>().FirstPersonCamera.transform.forward, out hit, maxDistance))
                {
                    //print(hit.transform.name);
                    InteractionManager interactable = hit.collider.GetComponent<InteractionManager>();
                    if (interactable != null && interactable.gameObject == this.gameObject)
                    {
                        //Display interaction available
                        //UIManager.ShowInteractionText(true);
                        //Accept input and trigger event
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            //Debug.Log("E pressed");
                            if (typeMenu == Type.Door)
                            {
                                disableWhenOpen.enabled = false;
                                ///GetComponent<Animator>().Play("Open");
                            }


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

                            if (typeMenu == Type.PickUp)
                            {
                                //Return back to main scene
                                Debug.Log("Picked up" + other.gameObject.name);
                                if(other.GetComponent<PlayerManager>().objectPlayerHas2 == null)
                                {
                                    other.GetComponent<PlayerManager>().objectPlayerHas2 = this.gameObject;
                                    //Destroy(this.gameObject);
                                    this.gameObject.SetActive(false);
                                } else
                                {
                                    print("Player is already carrying an item = " + other.GetComponent<PlayerManager>().objectPlayerHas2);
                                }
                            }

                            if(typeMenu == Type.DropOff)
                            {
                                print("try to drop off");
                                GetComponent<DropOff>().droppingOffItem(other.GetComponent<PlayerManager>().objectPlayerHas2, other.GetComponent<PlayerManager>(), gameObject.transform.position);
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
            if (other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                //Display interaction not available
                //UIManager.ShowInteractionText(false);
            }
        }
    }
}
