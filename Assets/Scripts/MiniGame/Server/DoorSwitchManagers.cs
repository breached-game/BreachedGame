using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitchManagers : MonoBehaviour
{
   
    private List<GameObject> doors;

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
        foreach (Transform child in transform)
        {
            doors.Add(child.gameObject);
        }
        UpdateDoors();
    }

    public void RecieveNewSwitchState(int switchID, int value)
    {
        currentSwitchState[switchID] = value;
        currentDoorState = combinationToDoor[currentSwitchState];
        UpdateDoors();
    }

    public void UpdateDoors()
    {
        for (int i = 0; i < currentDoorState.Count; i++)
        {
            if (currentDoorState[i] == 0)
            {
                // close doors[i]
                transform.gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                // open doors[i]
                transform.gameObject.transform.GetChild(i).gameObject.SetActive(true);
            }

        }

    }












    // 8 combinations, 3 need to open single door. Others need to troll
    // 3 switches, either 0 or 1





}
