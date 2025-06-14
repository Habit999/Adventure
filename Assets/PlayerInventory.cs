using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// Controls the player inventory
/// </summary>

public class PlayerInventory : MonoBehaviour
{
	[HideInInspector] public PlayerController Controller;
	
	public IDictionary<GameObject, int> CollectedItems = new Dictionary<GameObject, int>();
	
	[HideInInspector] public int AvailableItemSlots = 4;

	public event Action OnInventoryChange;
	public event Action OnHotbarChange;

    public IDictionary<GameObject, int> HotbarItemOrder = new Dictionary<GameObject, int>();
	
	[HideInInspector] public int SelectedInvSlot;
	
	[HideInInspector] public GameObject EquippedItem;
	
	[SerializeField] private Transform rightHandSpot;
	
	void OnDisable()
	{
		OnHotbarChange = null;
        OnInventoryChange = null;
    }
	
	void Start()
	{
		Controller = GetComponent<PlayerController>();

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

		if(SelectedInvSlot == position) EquipItem();

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
		Item itemToAdd = item.GetComponent<Item>();
		bool alreadyAdded = false;

        foreach (GameObject invItem in CollectedItems.Keys)
		{
			if(itemToAdd.ItemData.Name == invItem.GetComponent<Item>().ItemData.Name)
			{
				if(CollectedItems[invItem] + amount <= itemToAdd.ItemData.MaxItemStack)
				{
					CollectedItems[invItem] += amount;
                    alreadyAdded = true;
                    break;
                }
            }
		}
		if(!alreadyAdded && CollectedItems.Count < AvailableItemSlots)
		{
			CollectedItems.Add(item, amount);
		}

		item.SetActive(false);

		OnInventoryChange?.Invoke();
    }

    // Check if there is space for item in inventory
    public bool CheckSpaceForItem(Item item)
	{

        foreach (GameObject invItem in CollectedItems.Keys)
        {
            if (item.ItemData.Name == invItem.GetComponent<Item>().ItemData.Name)
            {
                if (CollectedItems[invItem] + 1 <= item.ItemData.MaxItemStack)
                {
                    return true;
                }
            }
        }
        if (CollectedItems.Count < AvailableItemSlots)
        {
            return true;
        }
        return false;
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
					OnHotbarChange?.Invoke();
                    return item;
				}
				else if(CollectedItems[item] - amount == 0)
				{
					CollectedItems.Remove(item);
					HotbarItemOrder.Remove(item);
					if(item == EquippedItem) RemoveItemFromHand();
                    OnInventoryChange?.Invoke();
                    OnHotbarChange?.Invoke();
                    return item;
				}
			}
		}
		return null;
	}
	
	public void UpdateInventoryStats()
	{
		// Adjust inventory slots to player level
		if(Controller.SkillsMngr.CurrentSkills.strength >= 1)
		{
			AvailableItemSlots = 4;
		}
		else if(Controller.SkillsMngr.CurrentSkills.strength >= 3)
		{
			AvailableItemSlots = 8;
		}
		else if(Controller.SkillsMngr.CurrentSkills.strength >= 5)
		{
			AvailableItemSlots = 12;
		}
		else if(Controller.SkillsMngr.CurrentSkills.strength >= 7)
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
