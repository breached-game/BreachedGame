using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageForOrientationManager : MonoBehaviour
{
    public GameObject OrientationManager;

    public void relayToManagerDinnerPlate(GameObject player)
    {
        player.GetComponent<PlayerNetworkManager>().DinnerPlate(player, OrientationManager);
    }
    public void relayToManagerBed(GameObject player)
    {
        player.GetComponent<PlayerNetworkManager>().Bed(player, OrientationManager);
    }
}
