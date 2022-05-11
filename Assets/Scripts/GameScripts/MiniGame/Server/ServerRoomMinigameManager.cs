using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerRoomMinigameManager : MonoBehaviour
{

    /*
     Handles the logic for the server minigame. Relays to the minigame manager when the task is completed.
     Contributors : Seth
     */
    private MinigameManager minigameManager;
    public GameObject doors;
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
        doors.GetComponent<DoorSwitchManagers>().OpenAllDoors();
    }
}
