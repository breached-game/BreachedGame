using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public GameObject bottomPos;
    public GameObject topPos;
    private Vector3 originalPos;

    public void ClimbLadder(GameObject player)
    {
        print("Started climbing");
        Animator playerAni = player.GetComponent<Animator>();
        playerAni.Play("StartClimbing");
        originalPos = player.transform.position;
        if ((player.transform.position - topPos.transform.position).magnitude < (player.transform.position - bottomPos.transform.position).magnitude)
        {
            player.transform.position = topPos.transform.position;
            StartCoroutine(PlayerClimb(topPos.transform.position, bottomPos.transform.position, player));
        }
        else { 
            player.transform.position = bottomPos.transform.position;
            StartCoroutine(PlayerClimb(bottomPos.transform.position, topPos.transform.position, player));
        }
        playerAni.SetBool("Ladder", true);      
    }

    IEnumerator PlayerClimb(Vector3 startPos, Vector3 stopPos, GameObject player)
    {
        float t = 0.01f;
        while (t <= 1)
        {
            player.transform.position = Vector3.Lerp(startPos, stopPos, t);
            yield return new WaitForSeconds(0.01f);
        }
        originalPos.y = stopPos.y;
        player.transform.position = originalPos;
        player.GetComponent<Animator>().SetBool("Ladder", false);
    }
}
