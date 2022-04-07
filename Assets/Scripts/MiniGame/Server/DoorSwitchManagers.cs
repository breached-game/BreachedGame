using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitchManagers : MonoBehaviour
{
   
    private List<GameObject> doors =new List<GameObject>();

    private Vector3Int currentSwitchState;
    private List<int>  currentDoorState;
    private Dictionary<Vector3Int, List<int>> combinationToDoor = new Dictionary<Vector3Int, List<int>>
    {
        {new Vector3Int(0,0,0), new List<int> {0,0,0,0,0} },
        {new Vector3Int(0,0,1), new List<int> {0,0,1,0,0} },
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
        UpdateDoors();
    }

    public void RecieveNewSwitchState(int switchID, int value)
    {
        // Need to client rpc the update to the doors !!! 
        print(currentSwitchState);
        currentSwitchState[switchID] = value;
        currentDoorState = combinationToDoor[currentSwitchState];
        UpdateDoors();
    }

    public void UpdateDoors()
    {
        for(int i = 0; i<currentDoorState.Count; i++)
        {
      
            if(currentDoorState[i] == 0) //Should be closed
            {

                StartCoroutine(doors[i].GetComponent<ServerDoorScript>().CloseDoor());
                
            }
            else   //Should be open
            {
                StartCoroutine(doors[i].GetComponent<ServerDoorScript>().OpenDoor());
            }

 


        }

    }

}
