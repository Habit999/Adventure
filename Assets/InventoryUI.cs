using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
	[HideInInspector] public InventoryManager InvManager;

	[SerializeField] private Transform itemRegion;
	
	[HideInInspector] public List<InventoryIcon> ItemIcons = new List<InventoryIcon>();
	
	public Color UnselectedItemColor;
	public Color SelectedItemColor;

	private Animator animator;

	private bool isOpen;

    void Awake()
	{
		InvManager = PlayerController.Instance.InventoryMngr;

        animator = GetComponent<Animator>();

		isOpen = false;

        // Set inventory icon list
        for (int i = 0; i < itemRegion.childCount; i++)
		{
			ItemIcons.Add(itemRegion.GetChild(i).gameObject.GetComponent<InventoryIcon>());
			ItemIcons[i].InventoryController = this;
        }
	}

    private void Update()
    {
        if(InvManager.Controller.PlayerState != PlayerController.PLAYERSTATE.InMenu && isOpen)
			ToggleOpen();
    }

    public void ToggleOpen()
	{
        isOpen = !isOpen;
        animator.SetBool("IsOpen", isOpen);
    }

    public void SlotClicked(int slotIndex)
	{
        if (slotIndex <= InvManager.CollectedItems.Count)
		{
            InvManager.SelectedInvSlot = slotIndex;
			RefreshInventorySlots();
        }
    }
	
	void RefreshInventorySlots()
	{
		int invItemCount = 0;
		for(int i = 0; i < InvManager.AvailableItemSlots; i++)
		{
			int itemIndex = 0;
			ItemIcons[i].gameObject.SetActive(true);
			foreach(GameObject invItem in InvManager.CollectedItems.Keys)
			{
				// Enabling active slots and setting item images
				if(itemIndex == invItemCount)
				{
					ItemIcons[i].UpdateItemImage(invItem.GetComponent<Item>().ItemData.Image);
					invItemCount++;
					break;
				}
				else 
				{
                    ItemIcons[i].UpdateItemImage(null);
                    itemIndex++;
				}
			}
		}
	}
}
