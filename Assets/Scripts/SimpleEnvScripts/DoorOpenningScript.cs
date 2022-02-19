using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenningScript : MonoBehaviour
{
    public float offsetToClose = 2;
    public float animationSpeed = 2;
    private bool doorOpen;
    private Vector3 endPoint;
    private Vector3 startPoint;
    private bool open = false;
    private int playersNearDoor = 0;
    private bool firstOpen = true;

    enum Type
    {
        NormalDoor,
        WaterDoor
    }
    [SerializeField] Type typeMenu;
    private void Start()
    {
        Transform doorTransfrom = transform;
        endPoint = new Vector3(doorTransfrom.position.x, doorTransfrom.position.y, doorTransfrom.position.z + offsetToClose);
        startPoint = new Vector3(doorTransfrom.position.x, doorTransfrom.position.y, doorTransfrom.position.z);
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(transform.position != endPoint) transform.position = Vector3.Lerp(transform.position, endPoint, Time.deltaTime * animationSpeed);
            open = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") playersNearDoor--;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") playersNearDoor++;
        if (firstOpen)
        {
            firstOpen = false;
            //Auto open doors
            if (typeMenu == Type.NormalDoor || typeMenu == Type.WaterDoor)
            {
                if (typeMenu == Type.WaterDoor)
                {
                    GetComponent<WaterDoor>().StartWater();
                }
            }
        }
    }
    void Update()
    {
        if(!open && transform.position != startPoint) transform.position = Vector3.Lerp(transform.position, startPoint, Time.deltaTime * animationSpeed);
        if (playersNearDoor == 0) open = false;
    }
}
