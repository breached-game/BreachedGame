using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "InteractionScriptableObject", menuName = "Interactions/StartButton")]
public class StartButtonSO : InteractionSO
{
     /*
        RUN BY THE INTERACTION MANAGER, MADE FOR THE START BUTTON IN THE LOBBY
        Contributors: Andrew Morgan and Seth Holdcroft
    */
    public override void RunInteraction(GameObject interactable, GameObject player)
    {
        // Checks whether the button has already been pressed
        if (interactable.transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            interactable.GetComponent<StartGameButton>().StartGame(player);
            interactable.transform.GetChild(0).GetComponent<Animator>().Play("Click");
        }
    }
}
