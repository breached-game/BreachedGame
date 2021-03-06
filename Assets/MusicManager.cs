using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioClip ambientMusic;
    public AudioClip intenseMusic;
    public AudioClip lobbyMusic;
    private string scene = "Lobby";
    private string currentScene;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Orientation" && scene != "Orientation")
        {
            audioSource.Stop();
            audioSource.clip = ambientMusic;
            audioSource.Play();
        }
        else if (currentScene == "StartGame" && scene != "StartGame")
        {
            audioSource.Stop();
            audioSource.clip = intenseMusic;
            audioSource.Play();
        }
        else if (currentScene == "EndGameWin" && scene != "Submarine")
        {
            audioSource.Stop();
            audioSource.clip = lobbyMusic;
            audioSource.Play();
        }
        scene = currentScene;
    }
}
