using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InteractionManager : NetworkBehaviour
{
    //We manage all interaction here
    [SyncVar]
    public bool avalible = true;

    public BoxCollider disableWhenOpen;

    enum Type
    {
        Door,
        MiniGame,
        Button,
        PickUp,
        DropOff
    };
    [SerializeField] Type typeMenu;

    //We need this to display text/any UI
    //private SubmarineUIManager UIManager;

    //Not recommeneded to use GameObject.Find
    private void Start()
    {
        //UIManager = GameObject.Find("Canvas").GetComponent<SubmarineUIManager>();

        //if (UIManager == null) Debug.LogError("Interaction Manager cannot find SubmarineUIManager. Either Canvas doesn't exist or doesn't have SubmarineUIManager comp");
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && avalible)
        {
            if (other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                Ray ray = other.gameObject.GetComponent<PlayerManager>().FirstPersonCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))
                {
                    InteractionManager intractable = hit.collider.GetComponent<InteractionManager>();
                    if (intractable != null && intractable.gameObject == this.gameObject)
                    {
                        //Display interaction avalible
                        //UIManager.ShowInteractionText(true);
                        //Accept input and tigger event
                        if (Input.GetAxis("Interact") == 1)
                        {
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
                            if (typeMenu == Type.Button)
                            {
                                //Return back to main scene
                                Debug.Log("Returned to main scene");
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
        if (other.gameObject.tag == "Player" && avalible)
        {
            if (other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                //Display interaction not avalible
                //UIManager.ShowInteractionText(false);
            }
        }
    }
}
