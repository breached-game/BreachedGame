using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "InteractionScriptableObject", menuName = "Interactions/PressureButton")]
public class PressureButtonSO : InteractionSO
{
     /*
        RUN BY THE INTERACTION MANAGER, MADE FOR THE PRESSURE BUTTON
        (NO LONGER IN THE GAME)
        Contributors: Andrew Morgan and Seth Holdcroft
    */
    public override void RunInteraction(GameObject interactable, GameObject player)
    {
        if (interactable.transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            // Networks alarm press
            player.GetComponent<PlayerNetworkManager>().PressureAlarmPress(interactable);
            interactable.transform.GetChild(0).GetComponent<Animator>().Play("Click");
        }
    }
}