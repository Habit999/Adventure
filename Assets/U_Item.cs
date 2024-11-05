using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U_Item : A_Item
{
	public SpecificData _specificData = new SpecificData();
	
	public override bool CollectItem()
	{
		switch(_itemData.Type)
		{
			case SE_ItemData.TYPE.Weapon:
				print("Item Collected");
				return PlayerController.Instance.InventoryMngr.AddItem(this.gameObject, 1);
				
			case SE_ItemData.TYPE.Healing:
				print("Item Collected");
				return PlayerController.Instance.InventoryMngr.AddItem(this.gameObject, 1);
				
			case SE_ItemData.TYPE.Experience:
				PlayerController.Instance.SkillsMngr.AddExperience(GetRandomExperience);
				return true;
				
			default:
				print("Default Collection ItemStatus");
				return false;
		}
	}
}
