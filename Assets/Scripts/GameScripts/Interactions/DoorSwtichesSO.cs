using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "InteractionScriptableObject", menuName = "Interactions/DoorSwtiches")]
public class DoorSwtichesSO : InteractionSO {
     /*
        RUN BY THE INTERACTION MANAGER, MADE FOR A GIVEN INTERACTABLE
        Contributors: Andrew Morgan and Seth Holdcroft
    */
    public override void RunInteraction(GameObject interactable, GameObject player)
    {
        ServerRoomDoorSwitch serverRoomSwitch = interactable.GetComponent<ServerRoomDoorSwitch>();
        serverRoomSwitch.OnLeverChange(player);
    }
}
