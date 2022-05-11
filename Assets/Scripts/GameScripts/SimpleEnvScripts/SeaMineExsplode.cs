using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaMineExsplode : MonoBehaviour
{
    /*
        COORDINATES THE SEA MINE EXPLODING IN THE CUT SCENE

        Contributors: Andrew Morgan
    */
    private void OnTriggerEnter(Collider other)
    {
        // Checks if the submarine has collided with the sea mine
        if(other.gameObject.transform.name == "Submarine")
        {
            // Plays the animation and destroys the sea mine
            GetComponent<Animator>().Play("Play");
            StartCoroutine(death());
        }
    }

    // Coroutine to destroy the sea mine after it has exploded
    IEnumerator death()
    {
        yield return new WaitForSeconds(0.3f);
        for(int i = 0; i < transform.childCount; i++)
        {
            if (i != 1) transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
