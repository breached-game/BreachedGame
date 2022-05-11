using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitchManagers : MonoBehaviour
{
    /*
     Controlls the state of the doors used in the server minigame
     Contributors : Seth
     */

    private List<GameObject> doors =new List<GameObject>();

    private Vector3Int currentSwitchState;
    private List<int>  currentDoorState;
    private List<int> previousDoorState;
    private bool success = false;
    private Dictionary<Vector3Int, List<int>> combinationToDoor = new Dictionary<Vector3Int, List<int>>
    {

       //Left handside if the state of the levers, the right hand side shows the resulant state of the doors
        {new Vector3Int(0,0,0), new List<int> {0,0,0,0,0} },
        {new Vector3Int(0,0,1), new List<int> {0,1,1,0,1} },
        {new Vector3Int(0,1,0), new List<int> {1,0,0,0,1} },
        {new Vector3Int(0,1,1), new List<int> {0,1,0,1,0} },
        {new Vector3Int(1,0,0), new List<int> {0,0,1,0,1} },
        {new Vector3Int(1,0,1), new List<int> {0,0,0,1,0} },
        {new Vector3Int(1,1,0), new List<int> {0,1,0,0,0} },
        {new Vector3Int(1,1,1), new List<int> {1,0,0,1,0} },
    };

    private void Start()
    {
        currentSwitchState = new Vector3Int(0, 0, 0);
        currentDoorState = combinationToDoor[currentSwitchState];
        for(int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            doors.Add(transform.GetChild(i).gameObject);
        }
        //UpdateDoors();
    }

    public void RecieveNewSwitchState(int switchID, int value)
    {
        // Need to client rpc the update to the doors !!! 
        if (!success)
        {
            previousDoorState = currentDoorState;
            currentSwitchState[switchID] = value;
            currentDoorState = combinationToDoor[currentSwitchState];
            UpdateDoors();
        }
    }
    // Used when the game is finished
    public void OpenAllDoors()
    {
        success = true;
        previousDoorState = currentDoorState;
        currentDoorState = new List<int> { 1, 1, 1, 1, 1 };
        UpdateDoors();
    }

    public void UpdateDoors()
    {
        for(int i = 0; i<currentDoorState.Count; i++)
        {
            if (currentDoorState[i] == 0 && previousDoorState[i] != 0)
            {

                StartCoroutine(doors[i].GetComponent<ServerDoorScript>().CloseDoor());
                
            }
            else if (currentDoorState[i] == 1 && previousDoorState[i] != 1)
            {
                StartCoroutine(doors[i].GetComponent<ServerDoorScript>().OpenDoor());
            }
        }

    }

}
