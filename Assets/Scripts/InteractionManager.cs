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
                Ray ray = other.gameObject.GetComponent<PlayerManager>().FirstPersonCamera.transform.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))
                {
                    InteractionManager interactable = hit.collider.GetComponent<InteractionManager>();
                    if (interactable != null && interactable.gameObject == this.gameObject)
                    {
                        //Display interaction available
                        //UIManager.ShowInteractionText(true);
                        //Accept input and trigger event
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            Debug.Log("E pressed");
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
                                GetComponent<ColourMiniGameButton>().buttonPressed();
                            }

                            if (typeMenu == Type.PickUp)
                            {
                                //Return back to main scene
                                Debug.Log("Picked up" + other.gameObject.transform.name);
                                if(other.GetComponent<PlayerManager>().objectPlayerHas != null)
                                {
                                    other.GetComponent<PlayerManager>().objectPlayerHas = this.gameObject.name;
                                    Destroy(this.gameObject);
                                } else
                                {
                                    print("Player already has item");
                                }
                            }

                            if(typeMenu == Type.DropOff)
                            {
                                GetComponent<DropOff>().droppingOffItem(other.GetComponent<PlayerManager>().objectPlayerHas, other.gameObject);
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
