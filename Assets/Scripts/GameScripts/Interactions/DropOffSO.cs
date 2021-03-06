using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractionScriptableObject", menuName = "Interactions/DropOff")]
public class DropOffSO : InteractionSO
{
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
