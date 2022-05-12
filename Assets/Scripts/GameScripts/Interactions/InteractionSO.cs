using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "InteractionScriptableObject", menuName = "Interactions/Interaction")]
public class InteractionSO : ScriptableObject
{
    /*
        RUN BY THE INTERACTION MANAGER, RunInteraction WILL ALWAYS WRITTEN OVER
        Contributors: Andrew Morgan and Seth Holdcroft
    */
    public virtual void RunInteraction(GameObject interactable, GameObject player)
    {
        Debug.Log("Generic Interaction -> Inherit and change this script");
    }
}
