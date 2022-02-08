using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnHolaCountChanged))]
    int holaCount = 0;

    Camera cam;
    public Interactable focus;
    void HandleMovement()
    {
        if (isLocalPlayer)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal * 0.1f, moveVertical * 0.1f, 0);
            transform.position = transform.position + movement;
        }
    }

    void Update()
    {
        HandleMovement();

        //if the right mouse button is pressed
        if (Input.GetAxis("Fire2")>0 && isLocalPlayer)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Interactable intractable = hit.collider.GetComponent<Interactable>();
                if (intractable != null)
                {
                    SetFocus(intractable);
                }
            }
        }

        //if the left mouse button is pressed 
        if (Input.GetAxis("Fire1") > 0 && isLocalPlayer)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                RemoveFocus();
            }
        }
    }

    void SetFocus(Interactable newFocus) {
        if(newFocus != focus)
        {
            focus = newFocus;
        }
        
    }
    void RemoveFocus()
    {
        focus = null;
    }

    public override void OnStartServer()
    {
        Debug.Log("Player has been spawned on the server!");
    }

    [Command]
    void Hola()
    {
        Debug.Log("Received Hola from Client!");
        holaCount += 1;
        ReplyHola();
    }

    [TargetRpc]
    void ReplyHola()
    {
        Debug.Log("Received Hola from Server!");
    }

    [ClientRpc]
    void TooHigh()
    {
        Debug.Log("Too high!");
    }

    void OnHolaCountChanged(int oldCount, int newCount)
    {
        Debug.Log($"We had {oldCount} holas, but now we have {newCount} holas!");
    }
}
