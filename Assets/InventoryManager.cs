using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class InventoryManager : MonoBehaviour
{
	[HideInInspector] public PlayerController Controller;
	
	public IDictionary<GameObject, int> CollectedItems = new Dictionary<GameObject, int>();
	
	[HideInInspector] public int AvailableItemSlots = 4;

	public event Action OnInventoryChange;
	public event Action OnHotbarChange;

    // To be used
    public IDictionary<GameObject, int> HotbarItemOrder = new Dictionary<GameObject, int>();
	
	[HideInInspector] public int SelectedInvSlot;
	
	[HideInInspector] public GameObject EquippedItem;
	
	[SerializeField] private Transform rightHandSpot;
	
	void OnDisable()
	{
		Controller.SkillsMngr.LevelUp -= UpdateInventoryStats;
		OnHotbarChange = null;
        OnInventoryChange = null;
    }
	
	void Start()
	{
		Controller = GetComponent<PlayerController>();

        Controller.SkillsMngr.LevelUp += UpdateInventoryStats;

        SelectedInvSlot = -1;

		UpdateInventoryStats();

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

		OnHotbarChange?.Invoke();
    }

    public void EquipItem()
	{
		if(EquippedItem != null)
		{
			RemoveItemFromHand();
			AddItemToHand();
		}
		else AddItemToHand();
	}
	
	
	// Add item to collected inventory
	public void AddItem(GameObject item, int amount)
	{
		foreach(GameObject invItem in CollectedItems.Keys)
		{
			if(item == invItem)
			{
				if(CollectedItems[item] + amount <= item.GetComponent<Item>().ItemData.MaxItemStack)
				{
					CollectedItems[item] += amount;
                }
            }
		}
		if(CollectedItems.Count < AvailableItemSlots)
		{
			CollectedItems.Add(item, amount);
		}

		item.SetActive(false);

		OnInventoryChange?.Invoke();
    }
	
	// Remove item from collected inventory
	public GameObject RemoveItem(GameObject item, int amount)
	{
		foreach(GameObject invItem in CollectedItems.Keys)
		{
			if(item == invItem)
			{
				if(CollectedItems[item] - amount > 0)
				{
					CollectedItems[item] -= amount;
                    OnInventoryChange?.Invoke();
                    return item;
				}
				else if(CollectedItems[item] - amount == 0)
				{
					CollectedItems.Remove(item);
					HotbarItemOrder.Remove(item);
					if(item == EquippedItem) RemoveItemFromHand();
                    OnInventoryChange?.Invoke();
                    return item;
				}
			}
		}
		return null;
	}
	
	void UpdateInventoryStats()
	{
		// Adjust inventory slots to player level
		if(Controller.SkillsMngr.CurrentSkills.strength == 1)
		{
			AvailableItemSlots = 4;
		}
		else if(Controller.SkillsMngr.CurrentSkills.strength == 3)
		{
			AvailableItemSlots = 8;
		}
		else if(Controller.SkillsMngr.CurrentSkills.strength == 5)
		{
			AvailableItemSlots = 12;
		}
		else if(Controller.SkillsMngr.CurrentSkills.strength == 7)
		{
			AvailableItemSlots = 16;
		}
	}

    private void RemoveItemFromHand()
    {
        EquippedItem.SetActive(false);
        EquippedItem.transform.parent = null;
        EquippedItem = null;
    }

    private void AddItemToHand()
    {
        foreach (GameObject item in HotbarItemOrder.Keys)
        {
            if (HotbarItemOrder[item] == UserInterfaceController.ActiveHotbarSlot - 1)
            {
                EquippedItem = item;
                item.transform.parent = rightHandSpot;
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
                item.SetActive(true);
				return;
            }
        }
    }
}
