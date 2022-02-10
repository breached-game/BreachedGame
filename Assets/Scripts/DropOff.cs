using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOff : MonoBehaviour
{
    public string itemToBeDroppedOff;

    public void droppingOffItem(string item, GameObject Player)
    {
        if(item == itemToBeDroppedOff)
        {
            print(itemToBeDroppedOff + " has been dropped off");
            Player.GetComponent<PlayerManager>().objectPlayerHas = null;
        }
        else
        {
            print("Player does not have " + item + "instead it has " + Player.GetComponent<PlayerManager>().objectPlayerHas);
        }
    }
}
