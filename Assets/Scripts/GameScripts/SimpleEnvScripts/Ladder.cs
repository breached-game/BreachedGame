using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public GameObject bottomPos;
    public GameObject topPos;
    private Vector3 originalPos;
    private Animator playerAni;
    public GameObject topStopPos;

    public void ClimbLadder(GameObject player)
    {
        playerAni = player.GetComponent<PlayerManager>().PlayerModel.GetComponent<Animator>();
        playerAni.Play("StartClimbing");
        originalPos = player.transform.position;
        playerAni.SetBool("Ladder", true);
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
        float t = 0.01f;
        while (t <= 1)
        {
            player.transform.position = Vector3.Lerp(startPos, stopPos, t);
            yield return new WaitForSeconds(0.01f);
            t += 0.01f;
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
        }
        playerAni.SetBool("Ladder", false);
    }
}
