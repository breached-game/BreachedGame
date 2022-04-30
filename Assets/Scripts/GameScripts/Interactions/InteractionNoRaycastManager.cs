using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class InteractionNoRaycastManager : NetworkBehaviour
{
    //We manage all interaction here
    [SyncVar]
    public bool available = true;

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
        //print("interacting");
        if (other.gameObject.tag == "Player" && available)
        {
            if (other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer && !other.GetComponent<PlayerManager>().disableInteractionsForMinigame)
            {
                //Okay so GetKeyDown actually sucks
                //We check to see if the player is already interacting via the animator
                Animator playerAni = other.gameObject.GetComponent<PlayerManager>().PlayerModel.GetComponent<Animator>();
                if (Input.GetAxis("Interact") == 1 && !playerAni.GetCurrentAnimatorStateInfo(0).IsName("Interact"))
                {
                    interactionText.SetActive(false);
                    playerAni.Play("Interact");
                    //Want access to interactable SO
                    interactionSO.RunInteraction(gameObject, other.gameObject);
                }
                else if (!gameObject.GetComponent<Door>().open)
                {
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
