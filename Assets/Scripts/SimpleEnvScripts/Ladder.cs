using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public GameObject climbPos;

    public void ClimbLadder(GameObject player)
    {
        player.transform.position = climbPos.transform.position;
        Animator playerAni = player.GetComponent<Animator>();
        playerAni.SetBool("Ladder", true);
        playerAni.Play("StartClimbing");
    }
}
