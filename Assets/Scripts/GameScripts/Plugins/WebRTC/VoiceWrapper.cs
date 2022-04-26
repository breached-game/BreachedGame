using UnityEngine;
using System.Runtime.InteropServices;

public class VoiceWrapper : MonoBehaviour
{

    [DllImport("__Internal")]
    public static extern void Hello();

    [DllImport("__Internal")]
    public static extern void start();
    
    [DllImport("__Internal")]
    public static extern void muteMic();

    [DllImport("__Internal")]
    public static extern void waterMicOn();

    [DllImport("__Internal")]
    public static extern void waterMicOff();
}
