using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerDoorScript : MonoBehaviour
{
  
    private float doorMoveTime = 1f;

    private Transform doorTransform;

    //Not the actual door, used to prevent players running through mid animation
    public GameObject doorColliderObject;
    private BoxCollider doorCollider;

    private Vector3 closedDoorPos;
    public Vector3 openDoorPos;

    private bool closed = true;


    public void Start()
    {
        doorTransform = gameObject.GetComponent<Transform>();
        doorCollider  = doorColliderObject.GetComponent<BoxCollider>();
        closedDoorPos = doorTransform.localPosition;

    }


    public IEnumerator OpenDoor()
    {
        if (!closed)
        {
            yield break;
        }

        closed = false;
        float current_t = 0f;
        Vector3 distanceToMove  = openDoorPos - closedDoorPos ;
  
        while (current_t < doorMoveTime)
        {
            current_t += Time.deltaTime;
            doorTransform.localPosition = closedDoorPos + distanceToMove * (current_t / doorMoveTime);
            yield return null;
        }

        doorCollider.enabled = false;

    }

    public IEnumerator CloseDoor()
    {
        if (closed)
        {
            yield break;
        }

        doorCollider.enabled = true;
        closed = true;
        float current_t = 0f;
        Vector3 distanceToMove = closedDoorPos - openDoorPos;

        while (current_t < doorMoveTime)
        {
            current_t += Time.deltaTime;
            doorTransform.localPosition = openDoorPos + distanceToMove * (current_t / doorMoveTime);
            yield return null;
        }

    }
}
