using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
	public PlayerController Controller { get { return gameObject.GetComponent<PlayerController>(); } }
	
	public IDictionary<GameObject, int> _collectedItems = new Dictionary<GameObject, int>();
	
	[HideInInspector] public int _itemSlotCount = 4;
	
	// To be used
	[HideInInspector] public IDictionary<GameObject, int> _hotbarItemOrder = new Dictionary<GameObject, int>();
	
	[HideInInspector] public int _selectedInvSlot;
	
	[HideInInspector] public GameObject _equippedItem;
	
	[SerializeField] Transform rightHandSpot;
	
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
		// Check if position is already assigned to another item
		foreach(GameObject checkedItem in _hotbarItemOrder.Keys)
		{
			if(_hotbarItemOrder[checkedItem] == position)
			{
				if(checkedItem != _collectedItems.Keys.ElementAt(_selectedInvSlot))
				{
					_hotbarItemOrder.Remove(checkedItem);
					break;
				}
			}
		}
		
		// Assign new position
		if(_hotbarItemOrder.ContainsKey(_collectedItems.Keys.ElementAt(_selectedInvSlot))) 
			_hotbarItemOrder[_collectedItems.Keys.ElementAt(_selectedInvSlot)] = position;
		else _hotbarItemOrder.Add(_collectedItems.Keys.ElementAt(_selectedInvSlot), position);
	}
	
	public void EquipItemRight()
	{
		UserInterfaceController controllerUI = UserInterfaceController.Instance;
		if(controllerUI._currentActiveActionKey > 0 && controllerUI._currentActiveActionKey <= controllerUI.MaxActionKeysInRow)
		{
			if(_equippedItem == null)
			{
				foreach(GameObject item in _hotbarItemOrder.Keys)
				{
					if(_hotbarItemOrder[item] == UserInterfaceController.Instance._currentActiveActionKey - 1)
					{
						_equippedItem = item;
						_equippedItem.transform.parent = rightHandSpot;
						_equippedItem.transform.position = rightHandSpot.position;
						_equippedItem.transform.rotation = rightHandSpot.rotation;
						_equippedItem.SetActive(true);
						break;
					}
				}
			}
			else
			{
				_equippedItem.SetActive(false);
				_equippedItem.transform.parent = null;
				_equippedItem = null;
				EquipItemRight();
			}
		}
		else
		{
			if(_equippedItem != null)
			{
				_equippedItem.SetActive(false);
				_equippedItem.transform.parent = null;
				_equippedItem = null;
			}
		}
	}
	
	
	// Add item to collected inventory
	public bool AddItem(GameObject item, int amount)
	{
		foreach(GameObject invItem in _collectedItems.Keys)
		{
			if(item == invItem)
			{
				if(_collectedItems[item] + amount <= item.GetComponent<Item>()._itemData.MaxItemStack)
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
	public void RemoveItem(GameObject item, int amount)
	{
		foreach(GameObject invItem in _collectedItems.Keys)
		{
			if(item == invItem)
			{
				if(_collectedItems[item] - amount > 0)
				{
					_collectedItems[item] -= amount;
					break;
				}
				else if(_collectedItems[item] - amount == 0)
				{
					_collectedItems.Remove(item);
					_hotbarItemOrder.Remove(item);
					if(item == _equippedItem) Destroy(item);
					_equippedItem = null;
					break;
				}
			}
		}
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
