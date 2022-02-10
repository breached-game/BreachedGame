using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void MainScene()
    {
        //SceneManager.LoadScene("Main");
        //SceneManager.UnloadScene;
        //apparently above is now obselete
        SceneManager.UnloadSceneAsync("MiniGame1");
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

    public void MiniGame1()
    {
        SceneManager.LoadScene("MiniGame1");
    }

}
