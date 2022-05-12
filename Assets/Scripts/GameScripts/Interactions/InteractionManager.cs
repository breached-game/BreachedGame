using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class InteractionManager : NetworkBehaviour
{
    /*
        SORTS OUT ALL INTERACTIONS, INCLUDING RAYCASTING AND PROXIMITY 
        RUNS INTERACTION SCRIPTABLE OBJECT IF INTERACTING 
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

    // Runs when player is close to the object
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && available)
        {
            // Checks that the player should be interacting
            if (other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer && !other.GetComponent<PlayerManager>().disableInteractionsForMinigame)
            {
                RaycastHit hit;
                int maxDistance = 10000;
                Transform cameraTransform = other.gameObject.GetComponent<PlayerManager>().FirstPersonCamera.transform;
                // We check to see if the player is already interacting via the animator
                Animator playerAni = other.gameObject.GetComponent<PlayerManager>().PlayerModel.GetComponent<Animator>();
                // Checks whether E has been pressed
                if (Input.GetKeyDown(KeyCode.E) && !playerAni.GetCurrentAnimatorStateInfo(0).IsName("Interact"))
                {
                    interactionText.SetActive(false);
                    playerAni.Play("Interact");
                    // Checks to see whether the player is actually looking at the object
                    if (Physics.Raycast(cameraTransform.transform.position, cameraTransform.forward, out hit, maxDistance))
                    {
                        InteractionManager interactable = hit.collider.GetComponent<InteractionManager>();
                        if (interactable != null && (hit.collider.gameObject == gameObject || hit.collider.gameObject == gameObject.transform.GetChild(0).gameObject))
                        {
                            // Runs object-specific action on SO
                            interactionSO.RunInteraction(gameObject, other.gameObject);
                        }
                    }
                }
                // Gives instruction on how to pick object up if players is looking at it
                else if (Physics.Raycast(cameraTransform.transform.position, cameraTransform.forward, out hit, maxDistance) && hit.collider.gameObject == gameObject && !pickedUp)
                {
                    interactionActualText.text = "Press e to interact with " + hit.collider.gameObject.name;
                    interactionText.SetActive(true);
                }
                else
                {
                    interactionText.SetActive(false);
                }
            } 
        } 
    }

    // Removes instruction if player is no longer near object
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
