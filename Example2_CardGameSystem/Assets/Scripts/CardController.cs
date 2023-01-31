using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
	//Card UIs
	[SerializeField]
	Image cardArt;
	[SerializeField]
	TextMeshProUGUI text_CardName;
	[SerializeField]
	TextMeshProUGUI text_Cost;
	[HideInInspector]
	public RectTransform rectTrans;

	//Card Manager
	[HideInInspector]
	public CardManager cardManager;

	//Hovering and Selecting Scale / Position changes
	[SerializeField]
	Vector3 hoveringScale = new Vector3(2, 2, 2);
	//When mouse hovering over, change of card y
	[SerializeField]
	float hoveringY = 200;
	//Scale during dragging
	[SerializeField]
	Vector3 selectedScale = new Vector3(1.2f, 1.2f, 1.2f);
	//Card moving speed
	[SerializeField]
	float lerpSpeed = 3;
	//Anchor point for card position
	[HideInInspector]
	public Vector2 targetPosition = Vector2.zero;
	//Anchor rotation
	[HideInInspector]
	public Quaternion targetRotation = new Quaternion();

	//Card Status
	bool mouseRollOver = false;
	bool allowSelect = false;
	bool isDragging = false;

	[HideInInspector]
	public CardProperty cardProperty;

	void Start () {
		//Wait until 1 sec after, start dragging
		rectTrans = GetComponent<RectTransform>();
		Invoke("AllowSelect", 1);
	}

	public void Init(CardProperty cP)
	{
		//Setup card UIs
		cardProperty = cP;
		cardArt.sprite = cP.art;
		text_CardName.text = cP.name;
		text_Cost.text = cP.cost.ToString();
	}

	void Update()
	{
		if ((!mouseRollOver && !isDragging) || (!allowSelect && mouseRollOver) || CardManager.gameStatus != GameStatus.Ready)
		{
			//Nothing happens || Card moving back but mouse roll over || Not in player phase
			rectTrans.anchoredPosition = Vector2.Lerp(rectTrans.anchoredPosition, targetPosition, Time.deltaTime * lerpSpeed);
			rectTrans.rotation = Quaternion.Slerp(rectTrans.rotation, rectTrans.parent.rotation * targetRotation, Time.deltaTime * lerpSpeed);
		}
		else if (allowSelect && mouseRollOver && !isDragging)
		{
			//Mouse Roll Over
			rectTrans.localScale = hoveringScale;
			rectTrans.anchoredPosition = new Vector2(targetPosition.x, hoveringY);
			rectTrans.localRotation = Quaternion.Euler(Vector3.zero);
		}
	}

	//Mouse Roll Over 
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (cardManager.currentSelectedCard == null)
		{
			mouseRollOver = true;

			cardManager.RelocateAllCards(this);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		mouseRollOver = false;
		rectTrans.localScale = Vector3.one;

		cardManager.RelocateAllCards();
	}

	//Drag Detection
	public void OnBeginDrag(PointerEventData eventData)
	{
		if (allowSelect && eventData.button == PointerEventData.InputButton.Left)
		{
			if (!isDragging)
			{
				isDragging = true;
				cardManager.currentSelectedCard = this;
			}
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (allowSelect && eventData.button == PointerEventData.InputButton.Left)
		{
			if (isDragging)
			{
				rectTrans.localScale = selectedScale;
				Vector3 globalMousePos;
				if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTrans, eventData.position, eventData.pressEventCamera, out globalMousePos))
				{
					rectTrans.position = globalMousePos;
				}
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (allowSelect && eventData.button == PointerEventData.InputButton.Left)
		{
			if (isDragging)
			{
				//End Drag
				isDragging = false;
				//Check if player drag the card to the right area
				if (CardManager.gameStatus == GameStatus.Ready)
				{
					//Taking effects
					if (cardManager.TakingEffectCheck())
					{
						//gameObject.SetActive(false);
					}
				}

				rectTrans.localScale = Vector3.one;
				allowSelect = false;
				Invoke("AllowSelect", 0.5f);
				cardManager.currentSelectedCard = null;
			}
		}
	}

	//Waiting for the cards movement stop
	void AllowSelect()
	{
		allowSelect = true;
	}
}
