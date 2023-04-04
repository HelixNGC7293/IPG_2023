using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ChatGPTWrapper;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]
    PersonalityDatabase personDB;
    [SerializeField]
    GameSettings settings;
    [SerializeField]
    ChatGPTConversation chatGPT;

    [SerializeField]
    TMP_InputField iF_PlayerTalk;
    [SerializeField]
    TextMeshProUGUI tX_AIReply;
    [SerializeField]
    NPCController npc;

    string npcName = "Coco";
    string playerName = "Player";

    float currentGameTimer = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        chatGPT._initialPrompt = personDB.personalities[settings.selectedIndex].initialPrompt;
        //chatGPT._initialPrompt = string.Format(chatGPT._initialPrompt, npcName, playerName) + initialPrompt_CommonPart;

        //Enable ChatGPT
        chatGPT.Init();

    }

    private void Start()
    {
        chatGPT.SendToChatGPT("{\"player_said\":" + "\"Hello! Who are you?\"}");

    }
    // Update is called once per frame
    void Update()
    {
		if (Input.GetButtonUp("Submit"))
		{
			SubmitChatMessage();
		}

        currentGameTimer += Time.deltaTime;

    }

	private void OnApplicationQuit()
	{
        print(currentGameTimer);
        settings.gameTimer += currentGameTimer;
        PlayerPrefs.SetFloat("GameTimer", settings.gameTimer);
    }

	public void ReceiveChatGPTReply(string message)
    {
        print(message);

        try
        {
            if (!message.EndsWith("}"))
            {
                if (message.Contains("}"))
                {
                    message = message.Substring(0, message.LastIndexOf("}") + 1);
                }
                else
                {
                    message += "}";
                }
            }
            NPCJSONReceiver npcJSON = JsonUtility.FromJson<NPCJSONReceiver>(message);
            string talkLine = npcJSON.reply_to_player;
            tX_AIReply.text = "<color=#ff7082>" + npcName + ": </color>" + talkLine;

            npc.ShowAnimation(npcJSON.animation_name);

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            string talkLine = "Don't say that!";
            tX_AIReply.text = "<color=#ff7082>" + npcName + ": </color>" + talkLine;
        }
    }

    public void SubmitChatMessage()
    {
        if (iF_PlayerTalk.text != "")
        {
            Debug.Log("Message sent: " + iF_PlayerTalk.text);
            chatGPT.SendToChatGPT("{\"player_said\":\"" + iF_PlayerTalk.text + "\"}");
            ClearText();
        }
    }

    void ClearText()
    {
        iF_PlayerTalk.text = "";
    }
}
