using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "InteractionScriptableObject", menuName = "Interactions/Door")]
public class DoorSO : InteractionSO
{
    public override void RunInteraction(GameObject interactable, GameObject Player)
    {
        Player.GetComponent<PlayerNetworkManager>().OpenDoor(interactable);
    }
}


