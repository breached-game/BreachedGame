using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerRoomMinigameManager : MonoBehaviour
{
    private MinigameManager minigameManager;

    public GameObject resetButton;

    public string minigameName;
    [TextArea]
    public string minigameObjective;

    void Start()
    {
        minigameManager = transform.parent.GetComponent<MinigameManager>();
        minigameManager.SendObjectiveData(minigameName, minigameObjective);
    }

    public void Success()
    {
        minigameManager.ObjectiveCompleted(minigameName, minigameObjective);
    }
}
