using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;
using Mirror;

public class PlayerManager : MonoBehaviour
{
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    public GameObject PlayerModel;
    public GameObject FirstPersonCamera;

    private NetworkIdentity identity;

    private CharacterController _controller;

    public GameObject objectPlayerHas = null;
    public GameObject torch;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        identity = GetComponent<NetworkIdentity>();
        Cursor.lockState = CursorLockMode.Locked;

        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);

        //Start Animation Control
        StartCoroutine(animationControll());

    }

    public void TurnOnAudio()
    {
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (identity.isLocalPlayer)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
            FirstPersonCamera.GetComponent<AudioListener>().enabled = true;
        }
    }

    #region Movement
    public float Speed = 4.0f;
    public float smooth = 5f;
    private float translation;
    private float straffe;


    void Update()
    {
        if (identity.isLocalPlayer)
        {
            // Input.GetAxis() is used to get the user's input
            // You can furthor set it on Unity. (Edit, Project Settings, Input)
            translation = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
            straffe = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
            Vector3 move = new Vector3(translation, 0, straffe);

            _controller.Move(translation *  transform.forward);
            _controller.Move(straffe *  transform.right);

            if (Input.GetKeyDown("escape"))
            {
                // turn on the cursor
                Cursor.lockState = CursorLockMode.None;
            }
            
            if (!_controller.isGrounded)
            {
                _controller.Move(new Vector3(0, -10, 0) * Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.G) && objectPlayerHas != null)
            {
                //Drop current item

                objectPlayerHas.transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z); //Currrently -1 for player height, objects will float
                objectPlayerHas.SetActive(true);
                if (objectPlayerHas.transform.name == "Torch")//Torch is different to generic object.
                {
                    torch.SetActive(false);
                }
                objectPlayerHas = null;
            }

            //check if we have torch 
            if (objectPlayerHas != null)
            {
                if (objectPlayerHas.transform.name == "Torch")
                {
                    torch.SetActive(true);
                }
            }
        }
    }
    IEnumerator animationControll()
    {
        while (true)
        {
            //Animation control
            Vector3 prevPos = transform.position;
            yield return new WaitForSeconds(0.5f);
            Vector3 actualPos = transform.position;

            if (prevPos == actualPos) PlayerModel.GetComponent<Animator>().SetBool("Moving", false);
            if (prevPos != actualPos) PlayerModel.GetComponent<Animator>().SetBool("Moving", true);
        }
    }
    #endregion
}
