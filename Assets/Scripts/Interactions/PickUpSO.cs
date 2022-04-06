using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractionScriptableObject", menuName = "Interactions/PickUp")]
public class PickUpSO : InteractionSO
{
    public override void RunInteraction(GameObject interactable, GameObject Player)
    {
        //if the player is not carrying anything
        if (Player.GetComponent<PlayerManager>().objectPlayerHas == null)
        {
            Debug.Log("Picked up " + interactable.name);
            Player.GetComponent<PlayerManager>().CallCmdPickupObject(interactable);
            
            if (interactable.transform.name == "WaterPumpItem")
            {
                Debug.Log("CHECK ZERO");
                Player.GetComponent<PlayerNetworkManager>().CallRemovePump(interactable);
            }
        }
        else
        {
            Debug.Log("Player is already carrying an item = " + Player.GetComponent<PlayerManager>().objectPlayerHas);
        }
    }
}
