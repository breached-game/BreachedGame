using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "InteractionScriptableObject", menuName = "Interactions/PickUp")]
public class PickUpSO : InteractionSO
{
     /*
        RUN BY THE INTERACTION MANAGER, MADE FOR ANY ITEM WHICH IS PICKED UP
        Contributors: Andrew Morgan and Seth Holdcroft
    */
    public override void RunInteraction(GameObject interactable, GameObject Player)
    {
        // Checks whether the player is carrying anything
        if (Player.GetComponent<PlayerManager>().objectPlayerHas == null)
        {
            // Networks the fact that a player has picked something up
            Player.GetComponent<PlayerManager>().CallCmdPickupObject(interactable);
            interactable.GetComponent<InteractionManager>().pickedUp = true;
            // Action is different for water pump as it also must stop pumping water
            if (interactable.transform.name == "WaterPump")
            {
                Player.GetComponent<PlayerNetworkManager>().CallRemoveWaterPump(interactable);
            }
        }
        else
        {
            Debug.Log("Player is already carrying an item = " + Player.GetComponent<PlayerManager>().objectPlayerHas);
        }
    }
}
