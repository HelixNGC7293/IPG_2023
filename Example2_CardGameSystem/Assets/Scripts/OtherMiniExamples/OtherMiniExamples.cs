using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OtherMiniExamples : MonoBehaviour
{
    //These are separate examples for class, unrelated to the card game example

    //***Example A - Getters/Setters 1
    private int health;
    private int maxHealth = 100;

    public int Health
    {
        get { return health; }
        set { health = Mathf.Clamp(value, 0, maxHealth); Debug.Log("Health Value Changes"); }
    }
    //public int Health { get;set; }

    //***Example B - Getters/Setters 2
    private int _score = 0;
    public int Score { get { return _score; } }

    public void AddPoints(int points)
    {
        _score += points;
    }


    //***Example C - Dictionary
    Dictionary<string, int> dict = new Dictionary<string, int>();

    //***Example D - Dictionary For Loop
    Dictionary<string, int> leaderboard = new Dictionary<string, int>()
    {
        { "Player 1", 100 },
        { "Player 2", 200 },
        { "Player 3", 300 }
    };

    //***Example E - Query
    private List<int> numbers = new List<int>() { 100, 200, 300, 400, 500 };

    //***Example F - Stack
    private Stack<string> history = new Stack<string>();

    //***Example G - Bubble Sort
    private int[] sortNumbers = { 5, 4, 1, 3, 2 };

    private void Start()
    {
        //***Example A
        Health = 110;
        print("Current Health Value will Clamp between 0-100: " + Health);

        //***Example B
        AddPoints(10);
        print("Current Score: " + Score);

        //***Example C
        dict.Add("Holy Sword Attack Damage", 98);
        dict.Add("Fireball Attack Damage", 98);
        print("Current Damage of Fireball: " + dict["Fireball Attack Damage"]);

        //***Example D
        foreach (KeyValuePair<string, int> playerScore in leaderboard)
        {
            Debug.Log("Current score of " + playerScore.Key + ": " + playerScore.Value);
        }

        //***Example E
        IEnumerable<int> highNumbers = from num in numbers
                                                where num >= 270
                                                orderby num descending // optional
                                                select num;

        foreach (int highNum in highNumbers)
        {
            Debug.Log("Current high number is " + highNum);
        }

        //***Example F
        history.Push("Page 1");
        history.Push("Page 2");
        history.Push("Page 3");
        string previousPage = history.Pop();
        Debug.Log("Going back to: " + previousPage);
        Debug.Log("Remain Pages: " + history.Count);


        //***Example G
        for (int i = 0; i < sortNumbers.Length - 1; i++)
        {
            for (int j = 0; j < sortNumbers.Length - i - 1; j++)
            {
                if (sortNumbers[j] > sortNumbers[j + 1])
                {
                    int temp = sortNumbers[j];
                    sortNumbers[j] = sortNumbers[j + 1];
                    sortNumbers[j + 1] = temp;
                }
            }
        }

        Debug.Log("Bubble Sort result is: \n");

        foreach (int sortNum in sortNumbers)
        {
            Debug.Log(sortNum);
        }
    }


}
