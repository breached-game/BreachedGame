using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CommandManager : MonoBehaviour
{
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
    // Start is called before the first frame update
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
        rectTransform.position = new Vector2(Screen.width/2, Screen.height - Screen.height/8);
        rectTransform.sizeDelta = new Vector2(Screen.width / 3, Screen.height / 4);
        hudRectTransform.sizeDelta = new Vector2(Screen.width / 5, Screen.height / 8);
        hudRectTransform.position = new Vector2(Screen.width / 2, Screen.height - hudRectTransform.sizeDelta.y / 2);
        textMesh.fontSize = Screen.height * 0.04f;
        if (!typing && messages.Count != 0)
        {
            typing = true;
            msg = (Message)messages.Dequeue();
            StartCoroutine(TypeMessage(msg));
        }
    }

    public void QueueMessage(string msg, bool captain=false)
    {
        Message message = new Message();
        message.msg = msg;
        message.captain = captain;
        messages.Enqueue(message);
    }

    private Color ChangeHudColour(Color c)
    {
        if (c.a == 0.8f)
        {
            c.a = 0.25f;
        }
        else
        {
            c.a = 0.8f;
        }
        return c;
    }

    IEnumerator TypeMessage(Message message)
    {
        Color colour = hudImage.color;
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
