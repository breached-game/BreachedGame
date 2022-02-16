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

    public GameObject objectPlayerHas;
    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        identity = GetComponent<NetworkIdentity>();
        Cursor.lockState = CursorLockMode.Locked;

        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (identity.isLocalPlayer)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
        }
        else
        {
            //lets disable all other audio listeners
            FirstPersonCamera.GetComponent<AudioListener>().enabled = false;
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);


        /*Legacy code from third person camera control
        //Camera Work stuff
        CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();


        if (_cameraWork != null)
        {
            if (identity.isLocalPlayer)
            {
                _cameraWork.OnStartFollowing();
            }
        }
        else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        }
        */
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

            /* Not in first person but it's useful in third
            //Player look where they move
            if (move != Vector3.zero)
            {
                //We use model because it has no rotation where as player model does
                //We flip the movement  
                PlayerModel.transform.parent.gameObject.transform.forward = move * -1;
            }
            */
            
            if (!_controller.isGrounded)
            {
                _controller.Move(new Vector3(0, -10, 0) * Time.deltaTime);
            }
            //Animation control

            //Getting animation to play when character moving
            if (move == new Vector3(0, 0, 0))
            {
                PlayerModel.GetComponent<Animator>().SetBool("Moving", false);
            }
            if (move != new Vector3(0, 0, 0))
            {
                PlayerModel.GetComponent<Animator>().SetBool("Moving", true);
            }
        }
    }
    #endregion
}
