using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "InteractionScriptableObject", menuName = "Interactions/Ladder")]
public class LadderSO : InteractionSO
{
    /*
        RUN BY THE INTERACTION MANAGER, MADE FOR THE LADDER BETWEEN TWO LEVELS
        Contributors: Sam Barnes-Thornton
    */
    public override void RunInteraction(GameObject interactable, GameObject Player)
    {
        interactable.GetComponent<Ladder>().ClimbLadder(Player);
    }
}
