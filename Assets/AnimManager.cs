using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimManager : MonoBehaviour
{
    public void StopUsingItem()
    {
    	if(PlayerController.Instance.InventoryMngr._equippedItem.GetComponent<Item>()._itemData.Type == Item.ItemData.TYPE.Consumable)
			PlayerController.Instance.InventoryMngr.RemoveItem(PlayerController.Instance.InventoryMngr._equippedItem, 1);
    }
}
