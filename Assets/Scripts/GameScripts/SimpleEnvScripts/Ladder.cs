using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    /*
        HANDLES THE INTERACTION WITH THE LADDER. COORDINATES ANIMATION WITH
        MOVEMEMENT UP AND DOWN

        Contributors: Sam Barnes-Thornton and Andrew Morgan
    */

    // Empty game object representing the bottom the ladder
    public GameObject bottomPos;
    // Empty game object representing the top of the ladder
    public GameObject topPos;
    private Animator playerAni;
    public GameObject topStopPos;

    public void ClimbLadder(GameObject player)
    {
        playerAni = player.GetComponent<PlayerManager>().PlayerModel.GetComponent<Animator>();
        playerAni.Play("StartClimbing");
        playerAni.SetBool("Ladder", true);
        // Checks if player is at the top or bottom of the ladder
        if ((player.transform.position - topPos.transform.position).magnitude < (player.transform.position - bottomPos.transform.position).magnitude)
        {
            player.transform.position = topPos.transform.position;
            StartCoroutine(PlayerClimb(topPos.transform.position, bottomPos.transform.position, player));
        }
        else { 
            player.transform.position = bottomPos.transform.position;
            StartCoroutine(PlayerClimb(bottomPos.transform.position, topPos.transform.position, player));
        }    
    }

    IEnumerator PlayerClimb(Vector3 startPos, Vector3 stopPos, GameObject player)
    {
        float dt = 0.005f;
        float t = 0.005f;
        // Lerping is used to gradually move player between top/bottom of ladder
        while (t <= 1)
        {
            player.transform.position = Vector3.Lerp(startPos, stopPos, t);
            yield return new WaitForSeconds(dt/10);
            // Multiplied by 3 to increase speed of climbing as can be slow in WebGL
            t += dt * 3;
        }
        playerAni.Play("StopClimbing");
        if (startPos == bottomPos.transform.position)
        {
            player.transform.position = topStopPos.transform.position;
            player.GetComponent<PlayerManager>().ResetSpeed();
        }
        else
        {
            player.transform.position = stopPos;
            player.GetComponent<PlayerManager>().ResetSpeed();
        }
        playerAni.SetBool("Ladder", false);
    }
}
