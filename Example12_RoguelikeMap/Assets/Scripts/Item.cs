using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    GameObject prefab_PickUpParticle;

    [SerializeField]
    Sprite[] sp_Items;

    [SerializeField]
    SpriteRenderer renderer_Sp;

    [SerializeField]
    int itemID = 0;


	public void Init(int ItemID)
	{
        itemID = ItemID;
        renderer_Sp.sprite = sp_Items[itemID];
    }

	// Update is called once per frame
	public void Pickup()
    {
        GameObject pc = Instantiate(prefab_PickUpParticle, transform.parent);
        pc.transform.position = transform.position;

        UIManager.instance.PickUpItem(itemID);

        Destroy(gameObject);
    }
}
