using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DropOff : NetworkBehaviour
{
    public string itemToBeDroppedOff;

    public GameObject dropOffPrefab;
    private GameObject itemDropped;
    public bool hasItem;
    [Command]
    public void CmdDropOff(GameObject player, Vector3 platformPosition)
    {
        droppingOffItem(player, platformPosition);
    }
    [ClientRpc]
    public void droppingOffItem(GameObject player, Vector3 platformPosition)
    {
        GameObject playerHas = player.GetComponent<PlayerManager>().objectPlayerHas;
        if (playerHas.name == dropOffPrefab.name)
        {
            //if the player has the right item to drop off
            print(dropOffPrefab + " has been dropped off");

            //respawn object 
            int height = (int)playerHas.transform.lossyScale.y;
            Vector3 heightVector =  new Vector3 (0, height, 0);
            playerHas.transform.position = platformPosition + heightVector/2; //This is for dropping it on specific platform

            //itemCarried.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
            playerHas.SetActive(true);
            //reset carry field
            player.GetComponent<PlayerManager>().objectPlayerHas = null;
            //Update player UI
            player.GetComponent<PlayerManager>().updateItemText();

            hasItem = true;
            itemDropped = playerHas;
            Debug.Log("The drop off zone now has the item: " + playerHas.name);

            //Tell manager that we have change of state
            transform.parent.GetComponent<DropOffMiniGameManager>().changeInState(true);
        }
        else
        {
            //when player doesn't have right item
            print("Player does not have " + playerHas + " instead it has " + player.GetComponent<PlayerManager>().objectPlayerHas.name);
        }
    }

    public void FixedUpdate()
    {
        if (itemDropped != null)
        {
            //if the item dropped is not active it means it is temporarily in a players inventory and so not on the dropzone
            if (!itemDropped.activeSelf)
            {
                hasItem = false;
                Debug.Log("The drop off zone no longer has the item: " + itemDropped);
                itemDropped = null;

                //Tell manager that we have change of state
                transform.parent.GetComponent<DropOffMiniGameManager>().changeInState(false);
            }
        }
    }
}
