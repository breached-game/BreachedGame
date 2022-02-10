using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InteractionManager : NetworkBehaviour
{
    //We manage all interaction here
    [SyncVar]
    public bool avalible = true;

    public BoxCollider disableWhenOpen;

    enum Type
    {
        Door,
        MiniGame,
        Button
    };
    [SerializeField] Type typeMenu;

    //We need this to display text/any UI
    //private SubmarineUIManager UIManager;

    //Not recommeneded to use GameObject.Find
    private void Start()
    {
        //UIManager = GameObject.Find("Canvas").GetComponent<SubmarineUIManager>();

        //if (UIManager == null) Debug.LogError("Interaction Manager cannot find SubmarineUIManager. Either Canvas doesn't exist or doesn't have SubmarineUIManager comp");
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && avalible)
        {
            if (other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                Ray ray = other.gameObject.GetComponent<PlayerManager>().FirstPersonCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))
                {
                    InteractionManager intractable = hit.collider.GetComponent<InteractionManager>();
                    if (intractable != null && intractable.gameObject == this.gameObject)
                    {
                        //Display interaction avalible
                        //UIManager.ShowInteractionText(true);
                        //Accept input and tigger event
                        if (Input.GetAxis("Interact") == 1)
                        {
                            if (typeMenu == Type.Door)
                            {
                                disableWhenOpen.enabled = false;
                                GetComponent<Animator>().Play("Open");
                            }

                            SceneChanger sceneChanger = gameObject.GetComponent<SceneChanger>();

                            if (typeMenu == Type.MiniGame)
                            {
                                //Change to minigame scene
                                Debug.Log("Interacting with minigame");
                                //sceneChanger.MiniGame1();
                                gameObject.GetComponent<SceneChanger>().MiniGame1();

                                //intractable.gameObject.GetComponent<MiniGame>().action;
                                // instead of having multiple if statements we want a minigame class 
                                // we can then do something like this this.gameobject.action()

                            }
                            if (typeMenu == Type.Button)
                            {
                                //Return back to main scene
                                Debug.Log("Returned to main scene");
                                gameObject.GetComponent<SceneChanger>().MainScene();
                            }
                        }
                    }
                }
            } //else UIManager.ShowInteractionText(false);
        } //else UIManager.ShowInteractionText(false);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && avalible)
        {
            if (other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                //Display interaction not avalible
                //UIManager.ShowInteractionText(false);
            }
        }
    }
}
