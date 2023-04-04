using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;
using TMPro;

public class GameManager : MonoBehaviour
{
    JSONLoader jsonLoader;

    [SerializeField]
    TextMeshProUGUI tX_Name;
    [SerializeField]
    Image img;
    // Start is called before the first frame update
    void Start()
    {
        jsonLoader = GetComponent<JSONLoader>();
        jsonLoader.jsonRefreshed += ReadJSON;
    }

	private void OnDestroy()
    {
        jsonLoader.jsonRefreshed -= ReadJSON;
    }

	// Update is called once per frame
	public void ReadJSON(JSONNode json)
    {
        print(json["abilities"]);
        print(json["abilities"][0]["ability"]["name"]);
        tX_Name.text = json["name"];
        string imageURL = json["sprites"]["other"]["home"]["front_default"];
        print(imageURL);
        StartCoroutine(DownloadImage(imageURL));
    }
    IEnumerator DownloadImage(string imageURL)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageURL);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            Debug.Log(request.error);
		else
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        }
    }
}
