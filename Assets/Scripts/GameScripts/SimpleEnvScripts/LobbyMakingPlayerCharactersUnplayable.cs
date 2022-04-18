using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMakingPlayerCharactersUnplayable : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerManager>().Speed = 0;
            other.gameObject.GetComponent<PlayerManager>().FirstPersonCamera.SetActive(false);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerManager>().Speed = other.gameObject.GetComponent<PlayerManager>().defaultSpeed;
            other.gameObject.GetComponent<PlayerManager>().FirstPersonCamera.SetActive(true);
        }
    }
}
