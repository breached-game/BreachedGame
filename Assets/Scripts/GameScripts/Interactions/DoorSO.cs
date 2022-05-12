using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "InteractionScriptableObject", menuName = "Interactions/Door")]
public class DoorSO : InteractionSO
{
     /*
        RUN BY THE INTERACTION MANAGER, MADE FOR THE DOORS UNDER THE CONTROL ROOM
        Contributors: Andrew Morgan and Seth Holdcroft
    */
    public override void RunInteraction(GameObject interactable, GameObject Player)
    {
        // Networks door being opened
        Player.GetComponent<PlayerNetworkManager>().OpenDoor(interactable);
    }
}


