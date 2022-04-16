using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlRodButton : MonoBehaviour
{
    public Vector3Int direction;
    public ControlRodTransport controller;
    private bool isPressed = false;

    public void PressedDown()
    {
        isPressed = true;
    }

    public void PressedUp()
    {
        isPressed = false;
    }

    public void Update()
    {
        if (isPressed)
        {
            controller.CallButtonPressed(direction);
        }
    }
}
