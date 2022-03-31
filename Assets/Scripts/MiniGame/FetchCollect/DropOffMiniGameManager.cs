using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffMiniGameManager : MonoBehaviour
{
    public string minigameName;
    [TextArea]
    public string minigameObjective;
    private MinigameManager minigameManager;

    public GameObject ItemDroppedOff;

    public int numberOfDropOffs = 2;
    private int dropsDone = 0;

    public void Reset()
    {
        minigameManager = transform.parent.GetComponent<MinigameManager>();
        minigameManager.SendObjectiveData(minigameName, minigameObjective);
        ItemDroppedOff.SetActive(false);
    }
    private void Start()
    {
        Reset();
    }

    public void changeInState(bool droppedOff)
    {
        if (droppedOff) dropsDone++;
        else dropsDone--;

        print(dropsDone);

        if(dropsDone == numberOfDropOffs)
        {
            minigameManager.ObjectiveCompleted(minigameName, minigameObjective);
            visualEffect();
        }
    }

    private void visualEffect()
    {
        ItemDroppedOff.SetActive(true);
    }
}
