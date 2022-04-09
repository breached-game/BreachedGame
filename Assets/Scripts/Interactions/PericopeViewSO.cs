using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PericopeViewScriptableObject", menuName = "Interactions/PericopeView")]
public class PericopeViewSO : InteractionSO
{
    public override void RunInteraction(GameObject interactable, GameObject player)
    {
        Debug.Log("Running interaction SO");
        interactable.GetComponent<PeriscopeView>().EnteredPeriscope(player);
        player.GetComponent<PlayerManager>().disableInteractionsForMinigame = true;
    }
}