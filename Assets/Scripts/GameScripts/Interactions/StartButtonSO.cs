using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "InteractionScriptableObject", menuName = "Interactions/StartButton")]
public class StartButtonSO : InteractionSO
{
    public override void RunInteraction(GameObject interactable, GameObject player)
    {
        if (interactable.transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            interactable.GetComponent<StartGameButton>().StartGame(player);
            interactable.transform.GetChild(0).GetComponent<Animator>().Play("Click");
        }
    }
}
