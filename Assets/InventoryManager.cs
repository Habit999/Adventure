using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	public PlayerController Controller { get { return gameObject.GetComponent<PlayerController>(); } }
	
	public IDictionary<GameObject, int> _collectedItems = new Dictionary<GameObject, int>();
	
	[HideInInspector] public int _itemSlotCount = 4;
	
	// To be used
	[HideInInspector] public IDictionary<GameObject, int> _hotbarItemOrder = new Dictionary<GameObject, int>();
	
	[HideInInspector] public int _selectedInvSlot;
	
	void Awake()
	{
		_selectedInvSlot = -1;
	}
	
	void Update()
	{
		UpdateInventoryStats();
	}
	
	public void AssignHotbarItem(int position)
	{
		int count = 0;
		foreach(GameObject checkedItem in _hotbarItemOrder.Keys)
		{
			if(count == position)
			{
				if(checkedItem != _collectedItems[_selectedInvSlot])
				{
					_hotbarItemOrder.Remove(checkedItem);
					_hotbarItemOrder.Add(_collectedItems[_selectedInvSlot], position);
					break;
				}
				else
				{
					_hotbarItemOrder.Add(_collectedItems[_selectedInvSlot], position);
					break;
				}
			}
			count++;
		}
	}
	
	// Add item to collected inventory
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
	
	// Remove item from collected inventory
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
		// Adjust inventory slots to player level
		if(Controller.SkillsMngr._playerLevel == 1)
		{
			_itemSlotCount = 4;
		}
		else if(Controller.SkillsMngr._playerLevel == 3)
		{
			_itemSlotCount = 8;
		}
		else if(Controller.SkillsMngr._playerLevel == 6)
		{
			_itemSlotCount = 12;
		}
		else if(Controller.SkillsMngr._playerLevel == 8)
		{
			_itemSlotCount = 16;
		}
	}
}
