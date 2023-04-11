using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridUnit : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer highLight;
    [SerializeField]
    Color[] highLightColor;
    [SerializeField]
    TextMeshPro tX_Info;
    [SerializeField]
    TextMeshPro tX_Move;
    [SerializeField]
    GameObject pathfindingIndicator;

    [HideInInspector]
    public int x;
    [HideInInspector]
    public int y;
    //0 = normal; 1 = forest; 2 = rock
    [HideInInspector]
    public int tileType;

    // Start is called before the first frame update
    public void Init(int inputX, int inputY, int tT)
    {
        x = inputX;
        y = inputY;
        tileType = tT;
        highLight.color = highLightColor[tileType];
        tX_Info.text = "X:" + x + "\nY:" + y;
    }

    private void OnMouseUp()
    {
        if (GridManager.instance.inMovingMode)
        {
            //Confirm movement
            if (highLight.color == highLightColor[3])
            {
                GridManager.instance.ConfirmMovement(this);
            }
			else
            {
                GridManager.instance.ConfirmMovement();
            }
             
        }
    }

    private void OnMouseEnter()
	{
        highLight.gameObject.SetActive(true);

		if (GridManager.instance.inMovingMode)
		{
            GridManager.instance.SetTargetLocation(this);
		}
    }

	private void OnMouseExit()
    {
        if (highLight.color != highLightColor[3])
        {
            highLight.gameObject.SetActive(false);
        }
    }

    public void DisplayReachableGrid()
    {
        highLight.color = highLightColor[3];
        highLight.gameObject.SetActive(true);
    }


    public void DisplayPathfinding(string displayContent)
    {
        tX_Move.text = displayContent;
        pathfindingIndicator.SetActive(true);
    }

    public void HidePathfinding()
    {
        pathfindingIndicator.SetActive(false);
    }

    public void ResetGrid()
    {
        tX_Move.text = "";
        pathfindingIndicator.SetActive(false);
        highLight.gameObject.SetActive(false);
        highLight.color = highLightColor[tileType];
    }
}
