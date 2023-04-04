using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(GetJsonFromUrl("https://jsonplaceholder.typicode.com/todos/1", ReceivedJson1));

        StartCoroutine(GetJsonFromUrl("https://jsonplaceholder.typicode.com/comments", ReceivedJson2));

    }

    IEnumerator GetJsonFromUrl(string url, System.Action<string> callback)
    {
        string jsonText;

        UnityWebRequest www = UnityWebRequest.Get(url);
        //Just in case, make the request header to be json type
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            jsonText = www.error;
        }
        else
        {
            jsonText = www.downloadHandler.text;
        }

        // Parse JSON data
        //Debug.Log(jsonText);
        callback(jsonText);
        www.Dispose();
    }

    public void ReceivedJson1(string jsonText)
    {
        //Convert it back to usable structure
        JsonReceiver1 receiver = JsonUtility.FromJson<JsonReceiver1>(jsonText);
        print(receiver.userId + " " + receiver.id + " " + receiver.title + " " + receiver.completed);
    }
    public void ReceivedJson2(string jsonText)
    {
        //Advanced case
        JsonReceiver2 receiver = JsonUtility.FromJson<JsonReceiver2>("{\"comments\":" + jsonText + "}");
        Comment[] comments = receiver.comments;

        foreach (Comment comment in comments)
        {
            print(comment.email + " : " + comment.body);
        }
    }
}
