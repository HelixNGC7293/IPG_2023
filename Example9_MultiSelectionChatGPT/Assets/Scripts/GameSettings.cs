using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
	public float gameTimer = 0;
	public int selectedIndex;

	public void Reset()
	{
		selectedIndex = -1;
		gameTimer = 0;
	}
}