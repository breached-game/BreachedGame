using UnityEngine;
using System.Runtime.InteropServices;

/*
CREATES AN INTERFACE FOR JAVASCRIPT FUNCTION TO BE CALLED FROM UNITY
Contributors: Daniel Savidge and Luke Benson 
*/

public class VoiceWrapper : MonoBehaviour
{
    //Hello world test
    [DllImport("__Internal")]
    public static extern void Hello();

    //Initialises voice connection
    [DllImport("__Internal")]
    public static extern void start();
    
    //Changes microphone state to and from muted/unmuted
    [DllImport("__Internal")]
    public static extern void muteMic();

    //Applies water muffled effect on users voice 
    [DllImport("__Internal")]
    public static extern void waterMicOn();

    //Disables water muffled effect on users voice 
    [DllImport("__Internal")]
    public static extern void waterMicOff();
}
