using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	[System.Serializable]
	public struct ItemData
	{
		public enum TYPE {Weapon, Consumable, Experience}
		public TYPE Type;
		
		public string Name;
		
		public Sprite Image;
		
		public int MaxItemStack;
	}
	public ItemData _itemData = new ItemData();
	
	[Space(10)]
	public string _animatorTriggerName;
	
	[Space(10)]
	public float _damage;
	public float _healthRecovery;
	public float _manaRecovery;
	
	[Space(10)]
	public SkillsManager.SkillsList _minimumStatsRequirements = new SkillsManager.SkillsList(1, 1, 1);
	
	public float GetRandomExperience { get { return Random.Range(5, 10); } }
	
	public bool CollectItem()
	{
		switch(_itemData.Type)
		{
			case ItemData.TYPE.Weapon:
				print("Item Collected");
				return PlayerController.Instance.InventoryMngr.AddItem(this.gameObject, 1);
				
			case ItemData.TYPE.Consumable:
				print("Item Collected");
				return PlayerController.Instance.InventoryMngr.AddItem(this.gameObject, 1);
				
			case ItemData.TYPE.Experience:
				PlayerController.Instance.SkillsMngr.AddExperience(GetRandomExperience);
				return true;
				
			default:
				print("Default Collection ItemStatus");
				return false;
		}
	}
	
	public bool UseItem()
	{
		switch(_itemData.Type)
		{
			case ItemData.TYPE.Weapon:
				PlayerController.Instance.CombatMngr.SwingWeapon();
				return true;
				
			case ItemData.TYPE.Consumable:
				PlayerController.Instance.HealPlayer(_healthRecovery, _manaRecovery);
				return true;
				
			default:
				break;
		}
		
		return false;
	}
}
