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
        currentSwitchState[switchID] = value;
        currentDoorState = combinationToDoor[currentSwitchState];
        UpdateDoors();
    }

    public void UpdateDoors()
    {
        for (int i = 0; i < currentDoorState.Count; i++)
        {
            Animator aniDoor = transform.GetChild(i).GetComponent<Animator>();
            if (currentDoorState[i] == 0)
            {
                if (aniDoor.GetCurrentAnimatorStateInfo(0).IsName("Closed"))
                {
                    aniDoor.GetComponent<Animator>().Play("Closed");
                }
            }
            else
            {
                if (aniDoor.GetCurrentAnimatorStateInfo(0).IsName("Open"))
                {
                    aniDoor.GetComponent<Animator>().Play("Open");
                }
            }

        }

    }












    // 8 combinations, 3 need to open single door. Others need to troll
    // 3 switches, either 0 or 1





}
