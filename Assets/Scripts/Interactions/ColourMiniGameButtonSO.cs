using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColourMiniGameButtonScriptableObject", menuName = "Interactions/ColourMiniGameButton")]
public class ColourMiniGameButtonSO : InteractionSO
{
    public override void RunInteraction(GameObject interactable, GameObject player)
    {
        Debug.Log("Running interaction SO");
        if (interactable.transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            Debug.Log("Calling CmdButtonPressed");
            player.GetComponent<PlayerNetworkManager>().ButtonPressed(interactable);

        }
    }
}
