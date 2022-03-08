using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ControlRodScriptableObject", menuName = "Interactions/ControlRod")]
public class ControlRodSO : InteractionSO
{
    public override void RunInteraction(GameObject interactable, GameObject player)
    {
        Debug.Log("Running interaction SO");
        player.GetComponent<PlayerManager>().FirstPersonCamera.SetActive(false);
    }
}
