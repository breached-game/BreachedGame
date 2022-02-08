using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class NameTagManager : NetworkBehaviour
{
    [SyncVar] public string playerName;
    private GameObject nameTag;
    private NetworkIdentity identity;

    // Start is called before the first frame update
    void Start()
    {
        identity = GetComponent<NetworkIdentity>();
        if (identity.isLocalPlayer) { return; }
        else
        {
            playerName = PlayerPrefs.GetString("PlayerName");
            SetName();
        }
    }
    private void SetName() => nameTag.transform.GetComponent<TextMeshProUGUI>().text = playerName;
}
