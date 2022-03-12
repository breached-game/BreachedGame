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


    public GameObject ControlRodHoleOnSucces;


    void Start()
    {
        controlRodPos = controlRod.transform.position;
        minigameManager = transform.parent.GetComponent<MinigameManager>();
        minigameManager.SendObjectiveData(minigameName, minigameObjective);
    }
        
    public void Success()
    {
        ControlRodHoleOnSucces.SetActive(true);
        controlRod.GetComponent<MeshRenderer>().enabled = false;
        controlRod.GetComponent<CapsuleCollider>().enabled = false;
        minigameManager.ObjectiveCompleted(minigameName, minigameObjective);
    }

    public void Failure()
    {
        controlRod.transform.position = controlRodPos;
        minigameManager.ObjectiveFailed(minigameName, controlRodTouchedFail);
    }
}
