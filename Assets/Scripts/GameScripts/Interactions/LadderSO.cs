using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "InteractionScriptableObject", menuName = "Interactions/Ladder")]
public class LadderSO : InteractionSO
{
    public override void RunInteraction(GameObject interactable, GameObject Player)
    {
        interactable.GetComponent<Ladder>().ClimbLadder(Player);
    }
}
