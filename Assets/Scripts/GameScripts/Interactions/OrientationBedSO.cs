using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "InteractionScriptableObject", menuName = "Interactions/OrientationBed")]
public class OrientationBedSO : InteractionSO
{
    public override void RunInteraction(GameObject interactable, GameObject Player)
    {
        interactable.GetComponent<StorageForOrientationManager>().relayToManagerBed(Player);
        interactable.GetComponent<BoxCollider>().enabled = false;
    }
}