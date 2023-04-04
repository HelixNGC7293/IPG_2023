using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI tX_Selected;
    [SerializeField]
    PersonalityDatabase personDB;
    [SerializeField]
    GameSettings settings;

    [SerializeField]
    ToggleGroup toggleGroup;
    [SerializeField]
    Toggle[] toggles;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            toggles[i].GetComponentInChildren<Text>().text = personDB.personalities[i].name;
        }

        //Load data
        settings.gameTimer = PlayerPrefs.GetFloat("GameTimer", 0);
        settings.selectedIndex = PlayerPrefs.GetInt("SelectedIndex", 0);
        tX_Selected.text = "Current Selected Personality: " + personDB.personalities[settings.selectedIndex].name;

        toggles[settings.selectedIndex].isOn = true;
    }
    public void StartGame()
    {
        PlayerPrefs.SetFloat("GameTimer", settings.gameTimer);
        PlayerPrefs.SetInt("SelectedIndex", settings.selectedIndex);

        SceneManager.LoadScene("GameState");
    }

    public void OnValueChanges()
	{
        var currentToggle = toggleGroup.ActiveToggles().FirstOrDefault();
        
        int currentSelectedIndex = 0;
        for(int i = 0; i < toggles.Length; i++)
		{
            if(currentToggle == toggles[i])
			{
                currentSelectedIndex = i;
                break;
			}
		}

        settings.selectedIndex = currentSelectedIndex;
        tX_Selected.text = "Current Selected Personality: " + personDB.personalities[settings.selectedIndex].name;
    }

}
