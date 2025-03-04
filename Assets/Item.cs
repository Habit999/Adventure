using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	[System.Serializable]
	public struct ItemDataStructure
	{
		public enum TYPE {Weapon, Consumable, Experience}
		public TYPE Type;
		
		public string Name;
		
		public Sprite Image;
		
		public int MaxItemStack;
	}
	public ItemDataStructure ItemData = new ItemDataStructure();

	
	[Space(10)]
	public string AnimatorTriggerName;
	
	[Space(10)]
	public float Damage;
	public float HealthRecovery;
	public float ManaRecovery;
		
	public float GetRandomExperience { get { return Random.Range(5, 10); } }
	
	public bool CollectItem()
	{
		switch(ItemData.Type)
		{
			case ItemDataStructure.TYPE.Weapon:
				print("Item Collected");
				PlayerController.Instance.InventoryMngr.AddItem(this.gameObject, 1);
				return true;
				
			case ItemDataStructure.TYPE.Consumable:
				print("Item Collected");
				PlayerController.Instance.InventoryMngr.AddItem(this.gameObject, 1);
				return true;
				
			case ItemDataStructure.TYPE.Experience:
				PlayerController.Instance.SkillsMngr.AddExperience(GetRandomExperience);
				return true;
				
			default:
				print("Default Collection ItemStatus");
				return false;
		}
	}
	
	public bool UseItem()
	{
		switch(ItemData.Type)
		{
			case ItemDataStructure.TYPE.Weapon:
				PlayerController.Instance.CombatMngr.SwingWeapon();
				return true;
				
			case ItemDataStructure.TYPE.Consumable:
				PlayerController.Instance.HealPlayer(HealthRecovery, ManaRecovery);
				return true;
				
			default:
				break;
		}
		
		return false;
	}
}
