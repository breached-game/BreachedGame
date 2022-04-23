using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InteractionNoRaycastManager : NetworkBehaviour
{
    //We manage all interaction here
    [SyncVar]
    public bool available = true;

    public InteractionSO interactionSO;

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
                    playerAni.Play("Interact");
                    //Want access to interactable SO
                    interactionSO.RunInteraction(gameObject, other.gameObject);
                }
            }
        }
    }
}
