using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	public PlayerController Controller { get { return gameObject.GetComponent<PlayerController>(); } }
	
	public IDictionary<GameObject, int> _collectedItems = new Dictionary<GameObject, int>();
	
	public int _itemSlotCount = 4;
	
	void Awake()
	{
		_collectedItems.Clear();
	}
	
	void Update()
	{
		UpdateInventoryStats();
	}
	
	public bool AddItem(GameObject item, int amount)
	{
		foreach(GameObject invItem in _collectedItems.Keys)
		{
			if(item == invItem)
			{
				if(_collectedItems[item] + amount <= item.GetComponent<A_Item>()._itemData.MaxItemStack)
				{
					_collectedItems[item] += amount;
					return true;
				}
			}
		}
		if(_collectedItems.Count < _itemSlotCount)
		{
			_collectedItems.Add(item, amount);
			return true;
		}
		return false;
	}
	
	public bool RemoveItem(GameObject item, int amount)
	{
		foreach(GameObject invItem in _collectedItems.Keys)
		{
			if(item == invItem)
			{
				if(_collectedItems[item] - amount > 0)
				{
					_collectedItems[item] -= amount;
					return true;
				}
				else if(_collectedItems[item] - amount == 0)
				{
					_collectedItems.Remove(item);
				}
				else return false;
			}
		}
		return false;
	}
	
	void UpdateInventoryStats()
	{
		// If player level is == to 5 then increase item slot number
		// Inventory Slots
		if(Controller.SkillsMngr._playerLevel == 0)
		{
			_itemSlotCount = 4;
		}
		else if(Controller.SkillsMngr._playerLevel == 1)
		{
			_itemSlotCount = 8;
		}
		else if(Controller.SkillsMngr._playerLevel == 5)
		{
			_itemSlotCount = 12;
		}
		else if(Controller.SkillsMngr._playerLevel == 10)
		{
			_itemSlotCount = 16;
		}
	}
}
