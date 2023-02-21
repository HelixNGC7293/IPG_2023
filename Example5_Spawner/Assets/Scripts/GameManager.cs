using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform player;

    [HideInInspector]
    public bool isGameOver = false;
    [SerializeField]
    public GameObject ui_GameOverPage;

    void Awake()
    {
        if(instance == null)
		{
            instance = this;
		}
        else
		{
            Destroy(gameObject);
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
	{
        isGameOver = true;
        ui_GameOverPage.SetActive(true);

    }

    public void RestartLevel()
	{
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
