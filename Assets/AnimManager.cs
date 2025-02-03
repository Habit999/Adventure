using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimManager : MonoBehaviour
{
    public void StopUsingItem()
    {
    	if(PlayerController.Instance.InventoryMngr.EquippedItem.GetComponent<Item>().ItemData.Type == Item.ItemDataStructure.TYPE.Consumable)
			PlayerController.Instance.InventoryMngr.RemoveItem(PlayerController.Instance.InventoryMngr.EquippedItem, 1);
    }
}
