using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherExamples : MonoBehaviour
{
    ItemClassExample item1 = new ItemClassExample("Magical Potion", 120);
    ItemStructExample itemA = new ItemStructExample("Magical Potion", 120);

    struct ItemStructExample
    {
        public string name;
        public int price;

        //Constructor
        public ItemStructExample(string n, int p)
        {
            name = n;
            price = p;
        }
    }

    class ItemClassExample
    {
        public string name = "Health Potion";
        public int price = 10;

        //Constructor
        public ItemClassExample(string n, int p)
	    {
            name = n;
            price = p;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //Class Object is reference type, which can't be dublicate by just using '='
        var item2 = item1;
        item2.name = "Sword";
        item2.price = 33;
        //item1 value will be changed (Reference Type)
        print("item1: " + item1.name + " " + item1.price);
        print("item2: " + item2.name + " " + item2.price);

        var itemB = itemA;
        itemB.name = "Sword";
        itemB.price = 33;
        //itemA value will NOT be changed (value type)
        print("itemA: " + itemA.name + " " + itemA.price);
        print("itemB: " + itemB.name + " " + itemB.price);
    }
}
