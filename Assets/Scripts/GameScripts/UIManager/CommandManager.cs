using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CommandManager : MonoBehaviour
{
    /*
        HANDLES THE CAPTAIN 'COMMAND LINE' AT THE TOP OF THE SCREEN
        BOTH ADJUSTS THE SIZE FOR SCREEN DIMENSIONS AND COORDINATES
        THE PRINTING OF MESSAGES

        Contributors: Sam Barnes-Thornton
     */

    // Struct to make the passing of captain's messages more efficient
    private struct Message
    {
        public string msg;
        public bool captain;
    }

    private TextMeshProUGUI textMesh;
    private bool typing = false;
    private Queue messages = new Queue();
    private RectTransform rectTransform;
    public GameObject hudTrap;
    private RectTransform hudRectTransform;
    private Image hudImage;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        textMesh = GetComponent<TextMeshProUGUI>();
        hudRectTransform = hudTrap.GetComponent<RectTransform>();
        hudImage = hudTrap.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Message msg;
        // Code below adjusts dimensions
        rectTransform.position = new Vector2(Screen.width/2, Screen.height - Screen.height/8);
        rectTransform.sizeDelta = new Vector2(Screen.width / 2.5f, Screen.height / 4);
        hudRectTransform.sizeDelta = new Vector2(Screen.width / 5, Screen.height / 7);
        hudRectTransform.position = new Vector2(Screen.width / 2, Screen.height - hudRectTransform.sizeDelta.y / 2);
        textMesh.fontSize = Screen.height * 0.045f;
        // Works it's way through the queue of messages, ensuring it is never trying to type multiple
        // messages at once
        if (!typing && messages.Count != 0)
        {
            typing = true;
            msg = (Message)messages.Dequeue();
            StartCoroutine(TypeMessage(msg));
        }
    }

    // Queues a message to be written onto the command line
    public void QueueMessage(string msg, bool captain=false)
    {
        Message message = new Message();
        message.msg = msg;
        message.captain = captain;
        messages.Enqueue(message);
    }

    // Coroutine to type a message on the command line as if it is
    // being typed by hand
    IEnumerator TypeMessage(Message message)
    {
        string msg = message.msg;
        string currentMsg = "";
        bool captain = message.captain;
        string pref;
        int msgLength = msg.Length;
        int i = 0;
        if (captain)
        {
            pref = "CAPTAIN: ";
        }
        else {pref = ""; }
        while (currentMsg.Length < msgLength)
        {
            if (i % 10 == 0)
            {
                hudImage.color = new Color(0,0,0,0.25f);
            }
            else if (i % 10 == 5)
            {
                hudImage.color = new Color(0, 0, 0, 0.5f);
            }
            currentMsg += msg[i];
            textMesh.text = pref + currentMsg;
            yield return new WaitForSeconds(0.05f);
            i++;
        }
        hudImage.color = new Color(0, 0, 0, 0.25f);
        textMesh.outlineWidth = 0.2f;
        yield return new WaitForSeconds(2f);
        textMesh.outlineWidth = 0f;
        textMesh.text = "";
        typing = false;
    }
}
