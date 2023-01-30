using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Serializable card property, stores key data of card, will be used during the instantiation of CardController
[Serializable]
public class CardProperty
{
    //Card name
    public string name = "";
    //Unique art sprite of the card
    public Sprite art;
    //Energy cost of using the card
    public int cost = 10;
}

//An array that stores all the card properties data
public class CardDatabase : MonoBehaviour
{
    public CardProperty[] cardData;
}
