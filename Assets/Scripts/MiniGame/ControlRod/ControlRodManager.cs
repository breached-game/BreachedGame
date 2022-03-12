using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlRodManager : MonoBehaviour
{
    private MinigameManager minigameManager;

    public GameObject controlRod;
    private Vector3 controlRodPos;

    public string minigameName;
    public string minigameObjective;

    public string controlRodTouchedFail;

    public GameObject X_controller;
    public GameObject Y_controller;
    public GameObject Z_controller;


    void Start()
    {
        controlRodPos = controlRod.transform.position;
        minigameManager = transform.parent.GetComponent<MinigameManager>();
        minigameManager.SendObjectiveData(minigameName, minigameObjective);
    }
        
    public void Success()
    {
        print("Success");
        controlRod.transform.position = controlRodPos;
        minigameManager.ObjectiveCompleted(minigameName, minigameObjective);
        X_controller.GetComponent<ControlRodTransport>().ExitController();
        Y_controller.GetComponent<ControlRodTransport>().ExitController();
        Z_controller.GetComponent<ControlRodTransport>().ExitController();
    }

    public void Failure()
    {
        controlRod.transform.position = controlRodPos;
        minigameManager.ObjectiveFailed(minigameName, controlRodTouchedFail);
    }
}
