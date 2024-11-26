using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	[System.Serializable]
	public struct ItemData
	{
		public enum TYPE {Weapon, Healing, Experience}
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
				
			case ItemData.TYPE.Healing:
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
		bool successfullyUsed = false;
		switch(_itemData.Type)
		{
			case ItemData.TYPE.Weapon:
				PlayerController.Instance.CombatMngr.SwingWeapon();
				break;
				
			case ItemData.TYPE.Healing:
				PlayerController.Instance.HealPlayer(_healthRecovery, _manaRecovery);
				successfullyUsed = true;
				break;
				
			default:
				break;
		}
		
		if(successfullyUsed)
		{
			PlayerController.Instance.InventoryMngr.RemoveItem(this.gameObject, 1);
			return true;
		}
		else return false;
	}
}
