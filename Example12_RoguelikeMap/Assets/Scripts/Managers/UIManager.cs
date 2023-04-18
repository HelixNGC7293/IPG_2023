using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
	public static UIManager instance;
	[SerializeField]
	GameManager gameManager;
	[SerializeField]
	Player player;

	[SerializeField]
	Animator anim_PickUpItem;
	[SerializeField]
	Image image_PickUpItem;
	[SerializeField]
	Sprite[] sp_PickUpItem;

	[SerializeField]
	Color[] color_Password;
	[SerializeField]
	Sprite[] sp_Password;
	[SerializeField]
	Image bg_Password;
	[SerializeField]
	TextMeshProUGUI tX_PasswordContent;

	[SerializeField]
	TextMeshProUGUI tX_RelicContent;

	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
	}
	// Use this for initialization
	void Start()
	{
		DisplayPasswordEnd();
		DisplayRelicNum();
	}
	public void PickUpItem(int itemID)
	{
		image_PickUpItem.sprite = sp_PickUpItem[itemID];
		anim_PickUpItem.SetTrigger("Play");

		gameManager.itemCollected++;
		DisplayRelicNum();
	}

	void DisplayRelicNum()
	{
		tX_RelicContent.text = gameManager.itemCollected + "/" + gameManager.itemMaximumNum;
	}

	public void DisplayPassword(string password)
	{
		bg_Password.sprite = sp_Password[1];
		tX_PasswordContent.color = color_Password[1];
		tX_PasswordContent.fontSize = 20f;
		tX_PasswordContent.text = password;
	}

	public void DisplayPasswordEnd()
	{
		bg_Password.sprite = sp_Password[0];
		tX_PasswordContent.color = color_Password[0];
		tX_PasswordContent.fontSize = 12f;
		tX_PasswordContent.text = "Password";
	}
	public void Escape()
	{
		if((float)gameManager.itemCollected / gameManager.itemMaximumNum >= 0.8f)
		{
			gameManager.GameOver(GameEnding.Humanity);
		}
		else
		{
			gameManager.GameOver(GameEnding.DirectEscape);
		}
	}
}
