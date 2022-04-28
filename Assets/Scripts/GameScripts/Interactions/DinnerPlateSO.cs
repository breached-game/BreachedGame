using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "InteractionScriptableObject", menuName = "Interactions/DinnerPlateSO")]
public class DinnerPlateSO : InteractionSO
{
    public override void RunInteraction(GameObject interactable, GameObject Player)
    {
        interactable.GetComponent<StorageForOrientationManager>().relayToManagerDinnerPlate(Player);
        interactable.GetComponent<BoxCollider>().enabled = false;
    }
}