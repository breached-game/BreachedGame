using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void MainScene()
    {
        //SceneManager.LoadScene("Main");
        SceneManager.UnloadScene
    }

    public void MiniGame1()
    {
        SceneManager.LoadScene("MiniGame1");
    }

}
