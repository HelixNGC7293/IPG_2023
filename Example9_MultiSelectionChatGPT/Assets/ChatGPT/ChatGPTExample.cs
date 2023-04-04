using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ChatGPTWrapper;
using System;
using UnityEngine.SceneManagement;

public class ChatGPTExample : MonoBehaviour
{
    public static ChatGPTExample instance;
    [SerializeField]
    ChatGPTConversation chatGPT;

    [SerializeField]
    TMP_InputField iF_PlayerTalk;
    [SerializeField]
    TextMeshProUGUI tX_AIReply;

    [SerializeField]
    string aiName = "AI Narrator";
    [SerializeField]
    string playerName = "Player";

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        //Enable ChatGPT
        chatGPT.Init();

    }

    private void Start()
    {
        chatGPT.SendToChatGPT("Hello! Let's begin!");

    }
    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyUp(KeyCode.Return))
		{
			SubmitChatMessage();
		}
	}

    public void ReceiveChatGPTReply(string message)
    {
        print(message);

        tX_AIReply.text = "<color=#ff7082>" + aiName + ": </color>" + message;
    }

    public void SubmitChatMessage()
    {
        if (iF_PlayerTalk.text != "")
        {
            chatGPT.SendToChatGPT(iF_PlayerTalk.text);
            ClearText();
        }
    }

    void ClearText()
	{
        iF_PlayerTalk.text = "";
    }
}
