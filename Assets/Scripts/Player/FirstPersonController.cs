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

    public int minAngle;
    public int maxAngle;
    public bool cameraEnabled = true;
    private bool shaking = false;

    private float originalSpeed;
    private float originalSprintSpeed;
    

    private NetworkIdentity identity;

    // Use this for initialization
    void Start()
    {
        identity = character.GetComponent<NetworkIdentity>();
        if (!identity.isLocalPlayer)
        {
            GetComponent<Camera>().enabled = false;
        }
    }

    public void StartShake()
    {
        shaking = true;
        originalSpeed = gameObject.transform.parent.transform.parent.GetComponent<PlayerManager>().Speed;
        originalSprintSpeed = gameObject.transform.parent.transform.parent.GetComponent<PlayerManager>().SprintSpeed;
        gameObject.transform.parent.transform.parent.GetComponent<PlayerManager>().Speed = 1f;
        gameObject.transform.parent.transform.parent.GetComponent<PlayerManager>().SprintSpeed = 1f;
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        Quaternion originalRotation = transform.rotation;
        float x;
        float y;
        while (shaking)
        {
            transform.rotation = originalRotation;
            x = Random.Range(-5f, 5f);
            y = Random.Range(-5f, 5f);
            transform.rotation = Quaternion.Euler(x + originalRotation.eulerAngles.x, y + originalRotation.eulerAngles.y, originalRotation.eulerAngles.z);
            yield return new WaitForSeconds(0.01f);
        }
        transform.rotation = originalRotation;
    }

    public void StopShake()
    {
        shaking = false;
        gameObject.transform.parent.transform.parent.GetComponent<PlayerManager>().Speed = originalSpeed;
        gameObject.transform.parent.transform.parent.GetComponent<PlayerManager>().SprintSpeed = originalSprintSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (identity.isLocalPlayer)
        {
            if (cameraEnabled && !shaking)
            {
                // md is mosue delta
                var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
                md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
                // the interpolated float result between the two float values
                smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
                smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
                // incrementally add to the camera look
                mouseLook += smoothV;

                // vector3.right means the x-axis
                mouseLook.y = Mathf.Clamp(mouseLook.y, minAngle, maxAngle);
                transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
                character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
            }
        }
    }
}