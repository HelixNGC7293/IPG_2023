using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameStatus { Start, Ready, End };
public class CardManager : MonoBehaviour
{
	public static GameStatus gameStatus;

	[SerializeField]
	CardController cardPrefab;
	[SerializeField]
	TextMeshProUGUI text_Message;
	[SerializeField]
	TextMeshProUGUI text_Energy;

	CardDatabase cardDatabase;

	RectTransform cardGroup;

	//Player's hand cards
	List<CardController> cards_Hand = new List<CardController>();
	//Cards waiting for drawing
	List<CardProperty> cards_DrawPile = new List<CardProperty>();
	List<CardProperty> cards_DiscardPile = new List<CardProperty>();

	bool isDrawingCards;
	int cardDrawingIndex = 0;
	float cardDrawingTimer = 0;
	float cardDrawingTimerTotal = 0.2f;

	[HideInInspector]
	public CardController currentSelectedCard;


	public int drawCardNum = 5;
	public int currentEnergy;
	public int defaultEnergy = 23;



	// Use this for initialization
	void Start () {
		//cardDB.CreateInitCards();
		cardGroup = GameObject.Find("CardGroup").GetComponent<RectTransform>();
		cardDatabase = GameObject.Find("CardDatabase").GetComponent<CardDatabase>();

		foreach(CardProperty cP in cardDatabase.cardData)
		{
			cards_DrawPile.Add(cP);
		}

		RoundStart();
	}
	
	// Update is called once per frame
	void Update () {
		//Card shows up one by one
		if (isDrawingCards)
		{
			if (cardDrawingTimer > cardDrawingTimerTotal)
			{
				if(cards_DrawPile.Count == 0)
				{
					//Refill cards from discard pile
					foreach (CardProperty cP in cards_DiscardPile)
					{
						cards_DrawPile.Add(cP);
					}
				}
				CardProperty cardProperty = cards_DrawPile[Random.Range(0, cards_DrawPile.Count)];
				CardController card;

				//Generate Player card
				cardDrawingTimer = 0;

				card = Instantiate(cardPrefab);
				RectTransform cardTrans = card.GetComponent<RectTransform>();
				cardTrans.SetParent(cardGroup);
				//Start point
				cardTrans.anchoredPosition3D = new Vector3(600, 0, 0);

				card.Init(cardProperty);
				card.cardManager = this;

				//Add the controller to cards_Hand
				cards_Hand.Add(card);
				cards_DrawPile.Remove(cardProperty);

				//Relocation
				RelocateAllCards();


				cardDrawingIndex++;
				if(cardDrawingIndex >= drawCardNum)
				{
					gameStatus = GameStatus.Ready;
					//Finished drawing cards
					isDrawingCards = false;
				}
			}
			else
			{
				cardDrawingTimer += Time.deltaTime;
			}
		}
	}

	public void RelocateAllCards(CardController selectedCard = null)
	{
		if (cards_Hand.Count > 0)
		{
			//Player cards
			if (CardManager.gameStatus == GameStatus.End)
			{
				for (int i = 0; i < cards_Hand.Count; i++)
				{
					//Round end, move all the cards to the left corner
					cards_Hand[i].targetPosition = new Vector2(-Screen.width * 0.6f, -100);
					cards_Hand[i].targetRotation = Quaternion.Euler(Vector3.zero);
				}
			}
			else if (cards_Hand.Count == 1)
			{
				cards_Hand[0].targetPosition = Vector2.zero;
				cards_Hand[0].targetRotation = Quaternion.Euler(Vector3.zero);
			}
			else
			{
				//Calculate interval
				float cardInterval = (Screen.width - 700) / (cards_Hand.Count - 1);
				//Between 50-150
				cardInterval = Mathf.Clamp(cardInterval, 30, 150);
				float middleIndex = (cards_Hand.Count - 1) * 0.5f;

				int selectedCardIndex = 0;
				if (selectedCard != null)
				{
					selectedCardIndex = cards_Hand.IndexOf(selectedCard);
				}

				for (int i = 0; i < cards_Hand.Count; i++)
				{
					CardController cardController = cards_Hand[i];
					Vector2 targetPosition = new Vector2((i - middleIndex) * cardInterval, Mathf.Abs(middleIndex - i) * cardInterval * -0.2f);
					if (selectedCard != null)
					{
						//Move away from selected card
						if (i < selectedCardIndex)
						{
							targetPosition.x -= 12000 / cardInterval;
						}
						else if (i > selectedCardIndex)
						{
							targetPosition.x += 18000 / cardInterval;
						}
					}
					cardController.targetPosition = targetPosition;
					cardController.targetRotation = Quaternion.Euler(0, 0, (middleIndex - i) * cardInterval * 0.08f);
				}
			}
		}
	}

	public void RoundStart()
	{
		gameStatus = GameStatus.Start;
		cards_Hand.Clear();
		//Get new cards
		cardDrawingIndex = 0;
		//cards_DrawPile = cards;
		isDrawingCards = true;

		currentEnergy = defaultEnergy;

		text_Message.text = "";
		text_Energy.text = currentEnergy.ToString();
	}
	
	public void RoundEnd()
	{
		if (gameStatus == GameStatus.Ready)
		{
			gameStatus = GameStatus.End;
			if (cards_Hand.Count > 0)
			{
				//Discard all the hand cards
				foreach (CardController cardController in cards_Hand)
				{

					cards_DiscardPile.Add(cardController.cardProperty);
				}
			}

			RelocateAllCards();
			Invoke("CleanHandsAndStartNextRound", 0.6f);
		}
	}

	void CleanHandsAndStartNextRound()
	{
		//Round end of special card ability
		foreach(CardController cardController in cards_Hand)
		{
			Destroy(cardController.gameObject);
		}
		cards_Hand.Clear();

		//Next Round
		RoundStart();
	}

	//Return if card is used
	public bool TakingEffectCheck()
	{
		if (currentSelectedCard != null)
		{
			CardProperty cardProperty = currentSelectedCard.cardProperty;
			//For special card, destroyed after use
			if (currentSelectedCard.rectTrans.anchoredPosition.y > 300 && currentEnergy >= currentSelectedCard.cardProperty.cost)
			{
				//Normal card, drag to the center of screen

				//Apply card ability
				currentEnergy -= currentSelectedCard.cardProperty.cost;
				//This card is used
				//Add this card to the discard pile
				MoveCardToDiscardPile(currentSelectedCard);

				text_Message.text = "You just used \"" + currentSelectedCard.cardProperty.name + "\" Card";
				text_Energy.text = currentEnergy.ToString();


				return true;
			}
		}
		return false;
	}

	void MoveCardToDiscardPile(CardController cardController)
	{
		cards_DiscardPile.Add(cardController.cardProperty);
		cards_Hand.Remove(cardController);
		Destroy(cardController.gameObject);

		RelocateAllCards();
	}
}
