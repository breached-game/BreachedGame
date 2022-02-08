using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InteractionManager : NetworkBehaviour
{
    //We manage all interaction here
    //checking if it's a Player and if it's our player
    [SyncVar]
    public bool avalible = true;
    bool isFocus = false;
    //We need this to display text/any UI
    private SubmarineUIManager UIManager;

    //Not recommeneded to use GameObject.Find
    private void Start()
    {
        UIManager = GameObject.Find("Canvas").GetComponent<SubmarineUIManager>();

        if (UIManager == null) Debug.LogError("Interaction Manager cannot find SubmarineUIManager. Either Canvas doesn't exist or doesn't have SubmarineUIManager comp");
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && avalible)
        {
            if (other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                //Display interaction avalible
                UIManager.ShowInteractionText(true);
                //Accept input and tigger event
                if (Input.GetAxis("Interact") == 1)
                {
                    if (isFocus)
                    {
                        Debug.Log("INTERACT");    
                    }
                    if (avalible) avalible = false;
                    else avalible = true;
                }
            } else UIManager.ShowInteractionText(false);
        } else UIManager.ShowInteractionText(false);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && avalible)
        {
            if (other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                //Display interaction not avalible
                UIManager.ShowInteractionText(false);
            }
        }
    }
}
