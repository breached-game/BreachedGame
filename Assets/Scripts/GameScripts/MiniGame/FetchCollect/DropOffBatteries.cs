using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffBatteries : MonoBehaviour
{
    public string minigameName;
    [TextArea]
    public string minigameObjective;
    private MinigameManager minigameManager;

    public Material Green;
    public Material Red;
    public GameObject Battery;
    public GameObject Battery2;
    public GameObject PowerLight;
    public GameObject PowerLight2;

    public int numberOfDropOffs = 2;
    private int dropsDone = 0;


    public void Reset()
    {
        minigameManager = transform.parent.GetComponent<MinigameManager>();
        minigameManager.SendObjectiveData(minigameName, minigameObjective);
        Battery.SetActive(false);
        Battery2.SetActive(false);
        PowerLight.GetComponent<MeshRenderer>().material = Red;
        PowerLight2.GetComponent<MeshRenderer>().material = Red;
    }
    private void Start()
    {
        Reset();
    }

    public void changeInState(bool droppedOff)
    {
        if (droppedOff)
        {
            dropsDone++;
            visualEffectOfBatteryDropOff();
        }
        else dropsDone--;

        print(dropsDone);

        if (dropsDone == numberOfDropOffs)
        {
            minigameManager.ObjectiveCompleted(minigameName, minigameObjective);
        }
        else
        {
            minigameManager.commandNetwork.GetComponent<CommandNetworkManager>().SendNetworkMessage("Great, you've found one battery now grab the other one as well!", true);
        }
    }
    private void visualEffectOfBatteryDropOff()
    {
        if (Battery.activeInHierarchy)
        {
            Battery2.SetActive(true);
            PowerLight2.GetComponent<MeshRenderer>().material = Green;
        }
        else
        {
            Battery.SetActive(true);
            PowerLight.GetComponent<MeshRenderer>().material = Green;
        }
    }
}
