using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryIcon : MonoBehaviour
{
	public enum ICONTYPE { ItemSlot, HotbarButton };
	[SerializeField] ICONTYPE IconType;
	
	public void IconClick()
	{
		switch(IconType)
		{
			case ICONTYPE.ItemSlot:
				if(transform.GetSiblingIndex() <= PlayerController.Instance.InventoryMngr._collectedItems.Count)
					PlayerController.Instance.InventoryMngr._selectedInvSlot = transform.GetSiblingIndex();
				break;
			
			case ICONTYPE.HotbarButton:
				if(PlayerController.Instance.InventoryMngr._selectedInvSlot >= 0 && PlayerController.Instance.InventoryMngr._selectedInvSlot < PlayerController.Instance.InventoryMngr._collectedItems.Count)
					PlayerController.Instance.InventoryMngr.AssignHotbarItem(transform.GetSiblingIndex());
				break;
			
			default:
				break;
		}
	}
}
