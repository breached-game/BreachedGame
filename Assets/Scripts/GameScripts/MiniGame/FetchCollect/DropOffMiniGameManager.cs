using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffMiniGameManager : MonoBehaviour
{
    public string minigameName;
    [TextArea]
    public string minigameObjective;
    public MinigameManager minigameManager;

    public GameObject ItemDroppedOff;

    public int numberOfDropOffs = 2;
    private int dropsDone = 0;
    public bool active;

    public void Reset()
    {
        minigameManager = transform.parent.GetComponent<MinigameManager>();
        minigameManager.SendObjectiveData(minigameName, minigameObjective);
        ItemDroppedOff.SetActive(false);
    }
    private void Start()
    {
        if (active)
        {
            Reset();
        }
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
            if (minigameName.Contains("Fix breach"))
            {
                minigameManager.commandNetwork.GetComponent<CommandNetworkManager>().SendNetworkMessage("Well done for fixing the breach...there's a pump in one of the stores that should get rid of the water.", true);
            }
        }
    }

    private void visualEffect()
    {
        ItemDroppedOff.SetActive(true);
    }
}
