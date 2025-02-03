using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
	public PlayerController Controller { get { return gameObject.GetComponent<PlayerController>(); } }
	
	public IDictionary<GameObject, int> CollectedItems = new Dictionary<GameObject, int>();
	
	[HideInInspector] public int AvailableItemSlots = 4;
	
	// To be used
	[HideInInspector] public IDictionary<GameObject, int> HotbarItemOrder = new Dictionary<GameObject, int>();
	
	[HideInInspector] public int SelectedInvSlot;
	
	[HideInInspector] public GameObject EquippedItem;
	
	[SerializeField] private Transform rightHandSpot;
	
	void OnEnable()
	{
		Controller.SkillsMngr.LevelUp += UpdateInventoryStats;
	}
	
	void OnDisable()
	{
		Controller.SkillsMngr.LevelUp -= UpdateInventoryStats;
	}
	
	void Awake()
	{
		SelectedInvSlot = -1;
	}
	
	public void AssignHotbarItem(int position)
	{
		// Check if position is already assigned to another item
		foreach(GameObject checkedItem in HotbarItemOrder.Keys)
		{
			if(HotbarItemOrder[checkedItem] == position)
			{
				if(checkedItem != CollectedItems.Keys.ElementAt(SelectedInvSlot))
				{
					HotbarItemOrder.Remove(checkedItem);
					break;
				}
			}
		}
		
		// Assign new position
		if(HotbarItemOrder.ContainsKey(CollectedItems.Keys.ElementAt(SelectedInvSlot))) 
			HotbarItemOrder[CollectedItems.Keys.ElementAt(SelectedInvSlot)] = position;
		else HotbarItemOrder.Add(CollectedItems.Keys.ElementAt(SelectedInvSlot), position);
	}
	
	public void EquipItem()
	{
		//UserInterfaceController controllerUI = UserInterfaceController.Instance;
		/*if(UserInterfaceController.ActiveHotbarSlot > 0 && UserInterfaceController.ActiveHotbarSlot <= UserInterfaceController.MaxActionKeysInRow)
		{
			if(EquippedItem == null)
			{
				foreach(GameObject item in HotbarItemOrder.Keys)
				{
					if(HotbarItemOrder[item] == UserInterfaceController.Instance.ActiveHotbarSlot)
					{
						EquippedItem = item;
						EquippedItem.transform.parent = rightHandSpot;
						EquippedItem.transform.localPosition = Vector3.zero;
						EquippedItem.transform.localRotation = Quaternion.identity;
						EquippedItem.SetActive(true);
						break;
					}
				}
			}
			else
			{
				EquippedItem.SetActive(false);
				EquippedItem.transform.parent = null;
				EquippedItem = null;
				EquipItemRight();
			}
		}
		else
		{
			if(EquippedItem != null)
			{
				EquippedItem.SetActive(false);
				EquippedItem.transform.parent = null;
				EquippedItem = null;
			}
		}*/

		void RemoveItemFromHand()
		{
            EquippedItem.SetActive(false);
			EquippedItem.transform.parent = null;
            EquippedItem = null;
        }

		void AddItemToHand()
		{
			foreach(GameObject item in HotbarItemOrder.Keys)
			{
				if(HotbarItemOrder[item] == UserInterfaceController.ActiveHotbarSlot)
				{
					EquippedItem = item;
					item.transform.parent = rightHandSpot;
					item.transform.localPosition = Vector3.zero;
					item.transform.localRotation = Quaternion.identity;
					item.SetActive(true);
					break;
				}
			}
		}

		if(UserInterfaceController.ActiveHotbarSlot == 0)
		{
			if(EquippedItem != null) RemoveItemFromHand();
		}
		else
		{
			if(EquippedItem != null)
			{
				RemoveItemFromHand();
				AddItemToHand();
			}
			else AddItemToHand();
		}
	}
	
	
	// Add item to collected inventory
	public bool AddItem(GameObject item, int amount)
	{
		foreach(GameObject invItem in CollectedItems.Keys)
		{
			if(item == invItem)
			{
				if(CollectedItems[item] + amount <= item.GetComponent<Item>().ItemData.MaxItemStack)
				{
					CollectedItems[item] += amount;
					return true;
				}
			}
		}
		if(CollectedItems.Count < AvailableItemSlots)
		{
			CollectedItems.Add(item, amount);
			return true;
		}
		return false;
	}
	
	// Remove item from collected inventory
	public void RemoveItem(GameObject item, int amount)
	{
		foreach(GameObject invItem in CollectedItems.Keys)
		{
			if(item == invItem)
			{
				if(CollectedItems[item] - amount > 0)
				{
					CollectedItems[item] -= amount;
					break;
				}
				else if(CollectedItems[item] - amount == 0)
				{
					CollectedItems.Remove(item);
					HotbarItemOrder.Remove(item);
					if(item == EquippedItem) Destroy(item);
					EquippedItem = null;
					break;
				}
			}
		}
	}
	
	void UpdateInventoryStats()
	{
		// Adjust inventory slots to player level
		if(Controller.SkillsMngr._currentSkills.strength == 1)
		{
			AvailableItemSlots = 4;
		}
		else if(Controller.SkillsMngr._currentSkills.strength == 3)
		{
			AvailableItemSlots = 8;
		}
		else if(Controller.SkillsMngr._currentSkills.strength == 5)
		{
			AvailableItemSlots = 12;
		}
		else if(Controller.SkillsMngr._currentSkills.strength == 7)
		{
			AvailableItemSlots = 16;
		}
	}
}
