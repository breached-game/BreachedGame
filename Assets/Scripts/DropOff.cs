using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOff : MonoBehaviour
{
    public string itemToBeDroppedOff;
    public GameObject dropOffPrefab;

    //public void droppingOffItem(string itemCarried, GameObject Player)
    //{
    //    if(itemCarried == itemToBeDroppedOff)
    //    {
    //        print(itemToBeDroppedOff + " has been dropped off");
    //        //reset carry field
    //        Player.GetComponent<PlayerManager>().objectPlayerHas = null;
    //        //respawn object 
    //        Instantiate(dropOffPrefab);
    //    }
    //    else
    //    {
    //        //when player doesn't have righ item
    //        print("Player does not have " + itemCarried + "instead it has " + Player.GetComponent<PlayerManager>().objectPlayerHas);
    //    }
    //}
    public void droppingOffItem(GameObject itemCarried, PlayerManager player, Vector3 platformPosition)
    {
        print("in drop off function");
        if (itemCarried.name == dropOffPrefab.name)
        {
            print("inside if");
            print(dropOffPrefab + " has been dropped off");
            
            
            //respawn object 
            Debug.Log("Platform position = "+ platformPosition);
            int height = (int)itemCarried.transform.lossyScale.y;
            Vector3 heightVector =  new Vector3 (0, height, 0);
            itemCarried.transform.position = platformPosition + heightVector/2;
            itemCarried.SetActive(true);
            //reset carry field
            player.objectPlayerHas2 = null;

            //Instantiate(dropOffPrefab,platformPosition,Quaternion.identity);
        }
        else
        {
            //when player doesn't have righ item
            print("inside else");
            print("Player does not have " + itemCarried+ "instead it has " + player.objectPlayerHas2.name);
        }
    }
}
