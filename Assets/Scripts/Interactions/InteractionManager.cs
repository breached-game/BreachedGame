using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InteractionManager : NetworkBehaviour
{
    //We manage all interaction here
    [SyncVar]
    public bool available = true;

    public InteractionSO interactionSO;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && available)
        {
            if (other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                //Okay so GetKeyDown actually sucks
                //We check to see if the player is already interacting via the animator
                Animator playerAni = other.gameObject.GetComponent<PlayerManager>().PlayerModel.GetComponent<Animator>();
                if (Input.GetAxis("Interact") == 1 && !playerAni.GetCurrentAnimatorStateInfo(0).IsName("Interact"))
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
                            //Want access to interactable SO

                            
                            interactionSO.RunInteraction(gameObject, other.gameObject);
                           

                            //Display interaction available
                        }
                    }
                }
            } 
        } 
    }

    //Assign authority 
    int playersInColliderCount = 0;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && available)
        {
            //other.gameObject is the local player
            //Display interaction not available
            //UIManager.ShowInteractionText(false);
            playersInColliderCount--;
            if (playersInColliderCount == 0 && other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                other.gameObject.GetComponent<PlayerManager>().CmdRemoveAurthority(this.gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (available)
            {
                if(other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    playersInColliderCount++;
                    other.gameObject.GetComponent<PlayerManager>().CmdAssignAurthority(this.gameObject);
                   
             
                    Debug.Log("This object has authority: " +  this.gameObject.GetComponent<NetworkIdentity>().hasAuthority);
                }
            }
        }
    }
}
