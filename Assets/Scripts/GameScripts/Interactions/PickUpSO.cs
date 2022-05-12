using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "InteractionScriptableObject", menuName = "Interactions/PickUp")]
public class PickUpSO : InteractionSO
{
     /*
        RUN BY THE INTERACTION MANAGER, MADE FOR A GIVEN INTERACTABLE
        Contributors: Andrew Morgan and Seth Holdcroft
    */
    public override void RunInteraction(GameObject interactable, GameObject Player)
    {
        //if the player is not carrying anything
        if (Player.GetComponent<PlayerManager>().objectPlayerHas == null)
        {
            Player.GetComponent<PlayerManager>().CallCmdPickupObject(interactable);
            interactable.GetComponent<InteractionManager>().pickedUp = true;
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
