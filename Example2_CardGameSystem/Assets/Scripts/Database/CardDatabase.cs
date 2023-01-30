using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CardProperty
{
    public string name = "";
    public Sprite art;
    public int cost = 10;
}

public class CardDatabase : MonoBehaviour
{
    public CardProperty[] cardData;
}
