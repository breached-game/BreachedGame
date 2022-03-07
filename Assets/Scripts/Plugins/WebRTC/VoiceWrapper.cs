using UnityEngine;
using System.Runtime.InteropServices;

public class VoiceWrapper : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    public static extern void Hello();
#endif
}
