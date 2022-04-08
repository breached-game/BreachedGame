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
    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        Message msg;
        rectTransform.sizeDelta = new Vector2(Screen.width / 3, Screen.height / 4);
        rectTransform.position = new Vector2(Screen.width / 6, Screen.height / 8);
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

    IEnumerator TypeMessage(Message message)
    {
        string msg = message.msg;
        bool captain = message.captain;
        int msgLength = msg.Length;
        int i = 0;
        if (captain)
        {
            textMesh.text = "CAPTAIN: ";
        }
        else {textMesh.text = ""; }
        while (textMesh.text.Length < msgLength)
        {
            textMesh.text += msg[i];
            yield return new WaitForSeconds(0.1f);
            i++;
        }
        yield return new WaitForSeconds(1f);
        textMesh.text = "";
        typing = false;
    }
}
