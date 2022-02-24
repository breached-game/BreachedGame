using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Item")]
public class ItemSO : ScriptableObject
{
    public GameObject itemPlayerHolds;
    public string itemName;
}
