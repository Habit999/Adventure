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
				if(transform.GetSiblingIndex() <= PlayerController.Instance.InventoryMngr.CollectedItems.Count)
					PlayerController.Instance.InventoryMngr.SelectedInvSlot = transform.GetSiblingIndex();
				break;
			
			case ICONTYPE.HotbarButton:
				if(PlayerController.Instance.InventoryMngr.SelectedInvSlot >= 0 && PlayerController.Instance.InventoryMngr.SelectedInvSlot < PlayerController.Instance.InventoryMngr.CollectedItems.Count)
					PlayerController.Instance.InventoryMngr.AssignHotbarItem(transform.GetSiblingIndex() + 1);
				break;
			
			default:
				break;
		}
	}
}
