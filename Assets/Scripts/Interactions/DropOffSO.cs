using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractionScriptableObject", menuName = "Interactions/DropOff")]
public class DropOffSO : InteractionSO
{
    public override void RunInteraction(GameObject interactable, GameObject player)
    {
        if (player.GetComponent<PlayerManager>().objectPlayerHas.name == "Wood")
        {
            player.GetComponent<PlayerNetworkManager>().StopBreach(interactable);
        }
        Debug.Log("Try to drop off");
        player.GetComponent<PlayerNetworkManager>().DropOff(interactable);
    }
}
