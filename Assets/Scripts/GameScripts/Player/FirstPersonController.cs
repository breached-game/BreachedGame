using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FirstPersonController : MonoBehaviour
{

    [SerializeField]
    public float sensitivity = 5.0f;
    [SerializeField]
    public float smoothing = 2.0f;
    // the chacter is the capsule
    public GameObject character;
    // get the incremental value of mouse moving
    private Vector2 mouseLook;
    // smooth the mouse moving
    private Vector2 smoothV;
    // Used for slowing the camera during shaking
    private float actualSensitivity;

    public int minAngle;
    public int maxAngle;
    public bool cameraEnabled = true;
    private bool shaking = false;

    private float originalSpeed;
    private float originalSprintSpeed;

    private Quaternion currentRotation;

    private PlayerManager playerManager;

    private NetworkIdentity identity;

    // Use this for initialization
    void Start()
    {
        identity = character.GetComponent<NetworkIdentity>();
        if (!identity.isLocalPlayer)
        {
            GetComponent<Camera>().enabled = false;
        }
        playerManager = gameObject.transform.parent.transform.parent.GetComponent<PlayerManager>();
    }

    public void StartShake()
    {
        if (gameObject.activeSelf)
        {
            shaking = true;
            if (identity.isLocalPlayer)
            {
                playerManager.Speed = 1f;
                playerManager.SprintSpeed = 1f;
            }
        }
    }

    public void StopShake()
    {
        shaking = false;
        playerManager.ResetSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        if (identity.isLocalPlayer)
        {
            if (cameraEnabled)
            {
                if (shaking) { 
                    actualSensitivity = sensitivity/5; 
                    playerManager.Speed = 1f;
                    playerManager.SprintSpeed = 1f;
                }
                else{ actualSensitivity = sensitivity; }
                // md is mosue delta
                var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
                md = Vector2.Scale(md, new Vector2(actualSensitivity * smoothing, actualSensitivity * smoothing));
                // the interpolated float result between the two float values
                smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
                smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
                // incrementally add to the camera look
                mouseLook += smoothV;
                // vector3.right means the x-axis
                mouseLook.y = Mathf.Clamp(mouseLook.y, minAngle, maxAngle);
                if (shaking)
                {
                    float x = Random.Range(-3f, 3f);
                    float y = Random.Range(-3f, 3f);
                    currentRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
                    transform.localRotation = Quaternion.Euler(x + currentRotation.eulerAngles.x, y + currentRotation.eulerAngles.y, currentRotation.eulerAngles.z);
                    character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
                }
                else
                {
                    transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
                    character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
                }
            }
        }
    }
}