using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "InteractionScriptableObject", menuName = "Interactions/DropOff")]
public class DropOffSO : InteractionSO
{
    public override void RunInteraction(GameObject interactable, GameObject player)
    {
        if (player.GetComponent<PlayerManager>().objectPlayerHas.name == "WaterPumpItem")
        {
            player.GetComponent<PlayerManager>().objectPlayerHas.GetComponent<WaterManager>().OutflowWater();
        }
        Debug.Log("Try to drop off");
        interactable.GetComponent<DropOff>().CmdDropOff(player, interactable.transform.position);
    }
}
