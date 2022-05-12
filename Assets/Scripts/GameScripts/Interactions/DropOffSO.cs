using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractionScriptableObject", menuName = "Interactions/DropOff")]
public class DropOffSO : InteractionSO
{
     /*
        RUN BY THE INTERACTION MANAGER, MADE FOR A GIVEN INTERACTABLE
        Contributors: Andrew Morgan and Seth Holdcroft
    */
    public override void RunInteraction(GameObject interactable, GameObject player)
    {
        GameObject objectPlayerHas = player.GetComponent<PlayerManager>().objectPlayerHas;
        if (objectPlayerHas != null)
        {
            if (objectPlayerHas.name == "Wood")
            {
                player.GetComponent<PlayerNetworkManager>().StopBreach(interactable);
            }
        }
        player.GetComponent<PlayerNetworkManager>().DropOff(interactable);
    }
}
