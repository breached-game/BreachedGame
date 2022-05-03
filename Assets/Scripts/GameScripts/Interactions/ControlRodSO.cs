using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ControlRodScriptableObject", menuName = "Interactions/ControlRod")]
public class ControlRodSO : InteractionSO
{
    public override void RunInteraction(GameObject interactable, GameObject player)
    {
        interactable.GetComponent<ControlRodTransport>().EnteredController(player);
    }
}
