using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOff : MonoBehaviour
{
    public string itemToBeDroppedOff;

    public void droppingOffItem(string itemCarried, GameObject Player)
    {
        if(itemCarried == itemToBeDroppedOff)
        {
            print(itemToBeDroppedOff + " has been dropped off");
            //reset carry field
            Player.GetComponent<PlayerManager>().objectPlayerHas = null;
            //respawn object 
        }
        else
        {
            print("Player does not have " + itemCarried + "instead it has " + Player.GetComponent<PlayerManager>().objectPlayerHas);
        }
    }
}
