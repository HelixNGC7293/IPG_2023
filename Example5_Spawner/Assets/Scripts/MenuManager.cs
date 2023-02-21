using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Eddy.Utilities;

public class MenuManager : MonoBehaviour
{
	[SerializeField]
	AudioClip bgm;
	// Start is called before the first frame update
	private void Start()
	{
		MusicManager.instance.SwitchMusic(bgm);
	}

	public void GameStart()
	{
		SceneManager.LoadScene("GameScene");
	}
}
