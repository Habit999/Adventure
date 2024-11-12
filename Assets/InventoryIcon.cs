using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryIcon : MonoBehaviour
{
	public enum ICONTYPE { ItemSlot, HotbarButton };
	
	public void IconClick()
	{
		switch(IconType)
		{
			case ICONTYPE.ItemSlot:
				if(transform.GetSiblingIndex() <= PlayerController.Instance.InventoryMngr._collectedItems.Count)
					PlayerController.Instance.InventoryMngr._selectedInvSlot = transform.GetSiblingIndex();
				break;
			
			case ICONTYPE.HotbarButton:
			
			default:
				break;
		}
	}
}
