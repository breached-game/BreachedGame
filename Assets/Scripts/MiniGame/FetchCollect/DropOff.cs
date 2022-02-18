using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOff : MonoBehaviour
{
    public string itemToBeDroppedOff;
    public GameObject dropOffPrefab;

    public void droppingOffItem(GameObject itemCarried, PlayerManager player, Vector3 platformPosition)
    {
        if (itemCarried.name == dropOffPrefab.name)
        {
            //if the player has the right item to drop off
            print(dropOffPrefab + " has been dropped off");
            //respawn object 
            int height = (int)itemCarried.transform.lossyScale.y;
            Vector3 heightVector =  new Vector3 (0, height, 0);
            itemCarried.transform.position = platformPosition + heightVector/2; //This is for dropping it on specific platform
            //itemCarried.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
            itemCarried.SetActive(true);
            //reset carry field
            player.objectPlayerHas = null;

        }
        else
        {
            //when player doesn't have right item
            print("Player does not have " + itemCarried+ "instead it has " + player.objectPlayerHas.name);
        }
    }
}
