using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "InteractionScriptableObject", menuName = "Interactions/DinnerPlateSO")]
public class DinnerPlateSO : InteractionSO
{
     /*
        RUN BY THE INTERACTION MANAGER, MADE FOR THE DINNER PLATE IN THE ORIENTATION SCENE
        Contributors: Andrew Morgan and Seth Holdcroft
    */
    public override void RunInteraction(GameObject interactable, GameObject Player)
    {
        Player.GetComponent<PlayerNetworkManager>().DinnerPlate(interactable);
    }
}