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
    }

    public void Failure()
    {
        controlRod.transform.position = controlRodPos;
        minigameManager.ObjectiveFailed(minigameName, minigameObjective);
    }
}
