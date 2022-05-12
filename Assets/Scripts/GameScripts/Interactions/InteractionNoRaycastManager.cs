using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class InteractionNoRaycastManager : NetworkBehaviour
{
     /*
        SORTS OUT ALL INTERACTIONS, ONLY PROXIMITY WITHOUT RAYCASTING
        RUNS INTERACTION SCRIPTABLE OBJECT IF INTERACTING 

        THIS WAS REQUIRED AS THE WATER DISRUPTED THE RAYCASTING FOR
        THE DOORS BELOW THE CONTROL ROOM

        Contributors: Andrew Morgan and Sam Barnes-Thornton
    */
    [SyncVar]
    public bool available = true;

    // Specific scriptable object for that object
    public InteractionSO interactionSO;
    public GameObject interactionText;
    private TextMeshProUGUI interactionActualText;
    public bool pickedUp = false;
    private void Awake()
    {
        interactionText = GameObject.Find("Canvas").gameObject.transform.Find("InteractText").gameObject;
        interactionText.SetActive(false);
        interactionActualText = interactionText.GetComponent<TextMeshProUGUI>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && available)
        {
            if (other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer && !other.GetComponent<PlayerManager>().disableInteractionsForMinigame)
            {
                //We check to see if the player is already interacting via the animator
                Animator playerAni = other.gameObject.GetComponent<PlayerManager>().PlayerModel.GetComponent<Animator>();
                // Checks whether E has been pressed
                if (Input.GetAxis("Interact") == 1 && !playerAni.GetCurrentAnimatorStateInfo(0).IsName("Interact"))
                {
                    // Player does not need to be looking at the object to interact with it
                    interactionText.SetActive(false);
                    playerAni.Play("Interact");
                    //Want access to interactable SO
                    interactionSO.RunInteraction(gameObject, other.gameObject);
                }
                else if (!gameObject.GetComponent<Door>().open)
                {
                    // Shows instructions if door is not open
                    interactionActualText.text = "Press e to interact with " + gameObject.name;
                    interactionText.SetActive(true);
                }
                else
                {
                    interactionText.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && available)
        {
            if (other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer && !other.GetComponent<PlayerManager>().disableInteractionsForMinigame)
            {
                interactionText.SetActive(false);
            }
        }
    }
}
