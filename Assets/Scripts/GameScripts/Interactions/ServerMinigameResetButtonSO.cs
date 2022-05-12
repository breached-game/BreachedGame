using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ServerMinigameResetButtonScriptableObject", menuName = "Interactions/ServerMinigameResetButton")]
public class ServerMinigameResetButtonSO : InteractionSO
{
     /*
        RUN BY THE INTERACTION MANAGER, MADE FOR THE SERVER RESET BUTTON
        Contributors: Andrew Morgan and Seth Holdcroft
    */
    public override void RunInteraction(GameObject interactable, GameObject player)
    {
        if (interactable.transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            // Networks the fact that the server button has been pressed
            player.GetComponent<PlayerNetworkManager>().ServerButtonPressed(interactable);
        }
    }
}
