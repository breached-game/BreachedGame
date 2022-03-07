using UnityEngine;
using System.Runtime.InteropServices;

public class VoiceWrapper : MonoBehaviour
{

    [DllImport("__Internal")]
    public static extern void Hello();

}
