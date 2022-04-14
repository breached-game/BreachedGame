using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "InteractionScriptableObject", menuName = "Interactions/DoorSwtiches")]
public class DoorSwtichesSO : InteractionSO {
    public override void RunInteraction(GameObject interactable, GameObject player)
    {
        ServerRoomDoorSwitch serverRoomSwitch = interactable.GetComponent<ServerRoomDoorSwitch>();
        serverRoomSwitch.OnLeverChange(player);
    }
}
