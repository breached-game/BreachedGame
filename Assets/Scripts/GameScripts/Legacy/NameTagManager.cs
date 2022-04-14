using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class NameTagManager : NetworkBehaviour
{
    public GameObject nameTag;

    public void SetName(string name) => nameTag.transform.GetComponent<TextMeshProUGUI>().text = name;
}
