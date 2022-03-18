using UnityEngine;
using System.Runtime.InteropServices;

public class VoiceWrapper : MonoBehaviour
{
    //public GameObject TempGameObject;

    [DllImport("__Internal")]
    public static extern void Hello();

    [DllImport("__Internal")]
    public static extern void start();

    //[DllImport("__Internal")]
    //public static extern void gotMessageFromServer();
    
    //[DllImport("__Internal")]
    //public static extern void setUpPeer();

    //    [DllImport("__Internal")]
    //    public static extern void ReturnToPlayer();


    //Function to refer to in javascript
    //    public void returnHello()
    //   {
    //        // Just to test javascript returning to unity 
    //        print("\n Hello back from javascript");
    //    }

}
