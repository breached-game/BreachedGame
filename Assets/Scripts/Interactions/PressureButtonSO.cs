using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "InteractionScriptableObject", menuName = "Interactions/PressureButton")]
public class PressureButtonSO : InteractionSO
{
    public override void RunInteraction(GameObject interactable, GameObject player)
    {
        if (interactable.transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            interactable.GetComponent<PressureAlarm>().CmdPressureAlarmPress();
            interactable.transform.GetChild(0).GetComponent<Animator>().Play("Click");
        }
    }
}