using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerDoorScript : MonoBehaviour
{
  
    private float doorMoveTime = 1f;

    private Transform doorTransform;
    private BoxCollider doorCollider;

    private Vector3 closedDoorPos;
    public Vector3 openDoorPos;



    public void Start()
    {
        doorTransform = gameObject.GetComponent<Transform>();
        doorCollider = gameObject.GetComponent<BoxCollider>();
        closedDoorPos = doorTransform.localPosition;

    }


    public IEnumerator OpenDoor()
    {
        print("Opening Door");
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
        print("Closing Door");
        doorCollider.enabled = true;
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
