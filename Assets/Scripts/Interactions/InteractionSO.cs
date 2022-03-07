using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "InteractionScriptableObject", menuName = "Interactions/Interaction")]
public class InteractionSO : ScriptableObject
{
    public virtual void RunInteraction(GameObject interactable, GameObject player)
    {
        Debug.Log("Generic Interaction -> Inherit and change this script");
    }
}
