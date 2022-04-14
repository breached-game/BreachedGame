using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class SubmarineUIManager : NetworkBehaviour
{
    //Interact UI
    public GameObject interactionText;   
    
    //Timer UI and variables
    public GameObject timerText;      
    public float timeRemaining = 10;

    //Game Over UI
    public GameObject gameOverMenu;
    public GameObject gameOverText;
    //Private Serialised vars that are in sync
    [SyncVar]
    private bool win = false;

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            float minutes = Mathf.FloorToInt(timeRemaining / 60);
            float seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.GetComponent<Text>().text = minutes + ":" + seconds;

            //Does anyone call win
            if (win)
            {
                EndGame(win);
            }
        }
        else
        {
            //Win will be negative so it'll be a loss
            //This is time running out
            EndGame(win);
        }
    }

    //Public Function to be accessed outside of this object
    public void ShowInteractionText(bool show)
    {
        if (show) interactionText.SetActive(true);
        else interactionText.SetActive(false);
    }

    private void EndGame(bool win)
    {
        //Stop the game
        Time.timeScale = 0;

        gameOverMenu.SetActive(true);

        if (win) gameOverText.GetComponent<Text>().text = "You Win";
        else gameOverText.GetComponent<Text>().text = "You Lose";
    }
}
