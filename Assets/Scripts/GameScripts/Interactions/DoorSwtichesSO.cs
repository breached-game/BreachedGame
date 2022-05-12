using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "InteractionScriptableObject", menuName = "Interactions/DoorSwtiches")]
public class DoorSwtichesSO : InteractionSO {
     /*
        RUN BY THE INTERACTION MANAGER, MADE FOR THE SERVER DOOR SWITCHES
        Contributors: Andrew Morgan and Seth Holdcroft
    */
    public override void RunInteraction(GameObject interactable, GameObject player)
    {
        ServerRoomDoorSwitch serverRoomSwitch = interactable.GetComponent<ServerRoomDoorSwitch>();
        // Actions are completed in another class
        serverRoomSwitch.OnLeverChange(player);
    }
}
