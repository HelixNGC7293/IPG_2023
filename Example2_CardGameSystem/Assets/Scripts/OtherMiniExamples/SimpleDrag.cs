using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleDrag : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
	[HideInInspector]
	public RectTransform rectTrans;
	bool isDragging = false;

	// Start is called before the first frame update
	void Start()
	{
		rectTrans = GetComponent<RectTransform>();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	//Drag Detection
	public void OnBeginDrag(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (!isDragging)
			{
				isDragging = true;
			}
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (isDragging)
			{
				Vector3 globalMousePos;
				if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTrans, eventData.position, eventData.pressEventCamera, out globalMousePos))
				{
					rectTrans.position = globalMousePos;
				}

				//Another way: control anchoredPosition Vector2 to make it follow the mouse
				//rectTrans.anchoredPosition = eventData.position - new Vector2(Screen.width / 2, Screen.height / 2);
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (isDragging)
			{
				//End Drag
				isDragging = false;
			}
		}
	}
}
