using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetServerButton : MonoBehaviour
{
    /*
     Script for button used to complete the Fix the server minigame 
     Contributors : Seth
     */
    public void ResetButtonPressed() 
    {
        transform.parent.GetComponent<ServerRoomMinigameManager>().Success();
        transform.GetChild(0).GetComponent<Animator>().Play("Click");
        GetComponent<InteractionManager>().interactionText.SetActive(false);
        gameObject.SetActive(false);
    }
}
