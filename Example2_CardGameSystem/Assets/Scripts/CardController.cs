using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{

	[SerializeField]
	Image cardArt;
	[SerializeField]
	TextMeshProUGUI text_CardName;
	[SerializeField]
	TextMeshProUGUI text_Cost;
	[SerializeField]
	Vector3 hoveredScale = new Vector3(2, 2, 2);
	[SerializeField]
	float hoveredHeight = 200;
	[SerializeField]
	Vector3 selectedScale = new Vector3(0.2f, 0.2f, 0.2f);


	bool mouseRollOver = false;
	bool allowSelect = false;
	bool isDragging = false;
	// Use this for initialization

	[HideInInspector]
	public CardManager cardManager;
	[HideInInspector]
	public Vector2 targetPosition = Vector2.zero;
	[HideInInspector]
	public Quaternion targetRotation = new Quaternion();
	//For TakingEffectCheck of cardManager
	[HideInInspector]
	public RectTransform rectTrans;

	public float lerpSpeed = 3;

	public CardProperty cardProperty;

	void Start () {
		rectTrans = GetComponent<RectTransform>();
		Invoke("AllowSelect", 1);
	}

	public void Init(CardProperty cP)
	{
		cardProperty = cP;
		cardArt.sprite = cP.art;
		text_CardName.text = cP.name;
		text_Cost.text = cP.cost.ToString();
	}

	void Update()
	{
		if ((!mouseRollOver && !isDragging) || (!allowSelect && mouseRollOver) || CardManager.gameStatus != GameStatus.Ready)
		{
			rectTrans.anchoredPosition = Vector2.Lerp(rectTrans.anchoredPosition, targetPosition, Time.deltaTime * lerpSpeed);
			rectTrans.rotation = Quaternion.Slerp(rectTrans.rotation, rectTrans.parent.rotation * targetRotation, Time.deltaTime * lerpSpeed);
		}
		else if (allowSelect && mouseRollOver && !isDragging)
		{
			rectTrans.localScale = hoveredScale;
			rectTrans.anchoredPosition = new Vector2(targetPosition.x, hoveredHeight);
			rectTrans.localRotation = Quaternion.Euler(Vector3.zero);
		}
	}
	
	void AllowSelect()
	{
		allowSelect = true;
	}

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
}
