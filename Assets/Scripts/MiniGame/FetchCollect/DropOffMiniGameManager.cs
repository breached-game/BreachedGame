using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffMiniGameManager : MonoBehaviour
{
    public string minigameName;
    public string minigameObjective;
    private MinigameManager minigameManager;

    public int numberOfDropOffs = 2;
    private int dropsDone = 0;

    private void Start()
    {
        minigameManager = transform.parent.GetComponent<MinigameManager>();
        minigameManager.SendObjectiveData(minigameName, minigameObjective);
    }

    public void changeInState(bool droppedOff)
    {
        if (droppedOff) dropsDone++;
        else dropsDone--;

        if(dropsDone == numberOfDropOffs)
        {
            minigameManager.ObjectiveCompleted(minigameName, minigameObjective);
        }
    }
}
