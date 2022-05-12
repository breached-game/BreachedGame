using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ControlRodScriptableObject", menuName = "Interactions/ControlRod")]
public class ControlRodSO : InteractionSO
{
     /*
        RUN BY THE INTERACTION MANAGER, MADE FOR THE CONTROL ROD CONTROLLERS
        Contributors: Andrew Morgan and Seth Holdcroft
    */
    public override void RunInteraction(GameObject interactable, GameObject player)
    {
        // Player enters controller and this is networked to all clients
        interactable.GetComponent<ControlRodTransport>().EnteredController(player);
    }
}
