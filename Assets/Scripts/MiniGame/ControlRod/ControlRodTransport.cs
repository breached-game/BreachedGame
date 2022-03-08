using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlRodTransport : MonoBehaviour
{
    public GameObject controlRodCamera;
    public GameObject controlRodUI;

    private void OnTriggerEnter(Collider other)
    {
        int numChildren;

        Cursor.lockState = CursorLockMode.None;
        other.gameObject.GetComponent<PlayerManager>().FirstPersonCamera.SetActive(false);
        //STOPPING PLAYER MOVING WHILE IN CONTROL ROD - MAYBE CHANGE PLAYERMANAGER TO HAVE A BOOL INSTEAD OF SETTING IT AS 0
        other.gameObject.GetComponent<PlayerManager>().Speed = 0.0f;
        controlRodCamera.SetActive(true);

        controlRodUI.SetActive(true);
        numChildren = controlRodUI.transform.childCount;
        for (int i = 0; i < numChildren; i++)
        {
            controlRodUI.transform.GetChild(i).gameObject.SetActive(true);
        }

        if (gameObject.name == "XMove")
        {
            controlRodUI.transform.GetChild(1).gameObject.SetActive(false);
            controlRodUI.transform.GetChild(2).gameObject.SetActive(false);
        } else if (gameObject.name == "YMove")
        {
            controlRodUI.transform.GetChild(0).gameObject.SetActive(false);
            controlRodUI.transform.GetChild(2).gameObject.SetActive(false);
        } else if (gameObject.name == "ZMove")
        {
            controlRodUI.transform.GetChild(0).gameObject.SetActive(false);
            controlRodUI.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

}
