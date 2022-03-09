using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlRodButton : MonoBehaviour
{
    public Vector3Int direction;
    public ControlRodTransport controller;
    // Start is called before the first frame update
    
    public void SendPressed()
    {
        controller.CallButtonPressed(direction);
    }
}
