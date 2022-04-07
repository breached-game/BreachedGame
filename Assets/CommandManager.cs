using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CommandManager : MonoBehaviour
{
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
        string msg;
        rectTransform.sizeDelta = new Vector2(Screen.width / 3, Screen.height / 4);
        rectTransform.position = new Vector2(Screen.width / 6, Screen.height / 8);
        if (!typing && messages.Count != 0)
        {
            print("typing");
            typing = true;
            msg = (string)messages.Dequeue();
            StartCoroutine(TypeMessage(msg));
        }
    }

    public void QueueMessage(string message)
    {
        messages.Enqueue(message);
    }

    IEnumerator TypeMessage(string message)
    {
        int msgLength = message.Length;
        int i = 0;
        textMesh.text = "";
        while (textMesh.text.Length < msgLength)
        {
            textMesh.text += message[i];
            yield return new WaitForSeconds(0.1f);
            i++;
        }
        yield return new WaitForSeconds(1f);
        textMesh.text = "";
        typing = false;
    }
}
