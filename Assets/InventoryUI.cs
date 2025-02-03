using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
	public InventoryManager InvManager { get { return PlayerController.Instance.InventoryMngr; } }
	
	public Transform ItemIconGroup { get { return transform.GetChild(0).GetChild(0); } }
	
	[HideInInspector] public List<Transform> _itemIcons = new List<Transform>();
	
	[HideInInspector] public Animator _invAnimator;
	
	[SerializeField] Color unselectedItemColor;
	[SerializeField] Color selectedItemColor;
	
	void Awake()
	{
		_invAnimator = GetComponent<Animator>();
		
		// Set inventory icon list
		_itemIcons.Clear();
		for(int i = 0; i < ItemIconGroup.childCount; i++)
		{
			_itemIcons.Add(ItemIconGroup.GetChild(i));
		}
	}
	
	void Update()
	{
		UpdateInventoryUI();
	}
	
	void UpdateInventoryUI()
	{
		int iconCount = 0;
		foreach(Transform icon in _itemIcons)
		{
			// Icon image enabling
			icon.gameObject.SetActive(false);
			icon.GetChild(0).gameObject.SetActive(false);
			
			// Setting selected item
			if(iconCount == InvManager.SelectedInvSlot) icon.gameObject.GetComponent<Image>().color = selectedItemColor;
			else icon.gameObject.GetComponent<Image>().color = unselectedItemColor;
			iconCount++;
		}
		int invItemCount = 0;
		for(int i = 0; i < InvManager.AvailableItemSlots; i++)
		{
			int itemIndex = 0;
			_itemIcons[i].gameObject.SetActive(true);
			foreach(GameObject invItem in InvManager.CollectedItems.Keys)
			{
				// Enabling active slots and setting item images
				if(itemIndex == invItemCount)
				{
					_itemIcons[i].GetChild(0).gameObject.SetActive(true);
					_itemIcons[i].GetChild(0).gameObject.GetComponent<Image>().sprite = invItem.GetComponent<Item>().ItemData.Image;
					invItemCount++;
					break;
				}
				else 
				{
					itemIndex++;
				}
			}
		}
	}
}
