using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaMineExsplode : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.transform.name == "Submarine")
        {
            GetComponent<Animator>().Play("Play");
            StartCoroutine(death());
        }
    }

    IEnumerator death()
    {
        yield return new WaitForSeconds(0.3f);
        for(int i = 0; i < transform.childCount; i++)
        {
            if (i != 1) transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
