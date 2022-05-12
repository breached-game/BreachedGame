using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractionScriptableObject", menuName = "Interactions/DropOff")]
public class DropOffSO : InteractionSO
{
     /*
        RUN BY THE INTERACTION MANAGER, MADE FOR ANY DROP OFF MINIGAMES
        Contributors: Andrew Morgan and Seth Holdcroft
    */
    public override void RunInteraction(GameObject interactable, GameObject player)
    {
        GameObject objectPlayerHas = player.GetComponent<PlayerManager>().objectPlayerHas;
        // Checks whether a player has an object
        if (objectPlayerHas != null)
        {
            // Slightly different outcome for wood as it stops a breach 
            if (objectPlayerHas.name == "Wood")
            {
                player.GetComponent<PlayerNetworkManager>().StopBreach(interactable);
            }
        }
        player.GetComponent<PlayerNetworkManager>().DropOff(interactable);
    }
}
