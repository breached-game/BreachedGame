using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    public Transform InteractionTransform;


    bool isFocus = false;
    Transform player;

    public virtual void Interact()
    {
        Debug.Log("Interacting with " + transform.name);
    }
    
    void Update()
    {
        if (isFocus)
        {
            float distance = Vector3.Distance(player.position, InteractionTransform.position);
            if (distance <= radius)
            {
                Debug.Log("INTERACT");
            }
        }    
    }

    public void OnFocused(Transform playerTransform) {
        isFocus = true;
        player = playerTransform;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(InteractionTransform.position, radius);

    }

}
