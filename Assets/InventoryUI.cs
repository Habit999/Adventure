using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
	[HideInInspector] public InventoryManager InventoryMngr;

	[SerializeField] private Transform itemRegion;
	[SerializeField] private Transform hotbarAssignmentRegion;
	
	[HideInInspector] public List<InventoryIcon> ItemIcons = new List<InventoryIcon>();
	
	public Color UnselectedItemColor;
	public Color SelectedItemColor;

	private Animator animator;

	private bool isOpen;

    private void Start()
    {
        RefreshInventorySlots();
    }

    void Awake()
	{
        animator = GetComponent<Animator>();

		isOpen = false;

        // Set inventory icon list
        foreach(var itemIcon in itemRegion.GetComponentsInChildren<InventoryIcon>())
		{
			ItemIcons.Add(itemIcon);
        }

		foreach(var hotbarAssignmentButton in hotbarAssignmentRegion.GetComponentsInChildren<InventoryIcon>())
		{
            hotbarAssignmentButton.InventoryUIController = this;
        }
    }

    private void Update()
    {
        if(InventoryMngr.Controller.PlayerState != PlayerController.PLAYERSTATE.InMenu && isOpen)
			ToggleOpen();
    }

    public void ToggleOpen()
	{
        isOpen = !isOpen;
        animator.SetBool("Open", isOpen);
    }

    public void SlotClicked(int slotIndex)
	{
        if (slotIndex <= InventoryMngr.CollectedItems.Count)
		{
            InventoryMngr.SelectedInvSlot = slotIndex;
			RefreshInventorySlots();
        }
    }

	public void RefreshInventorySlots()
	{
		foreach (var slot in ItemIcons)
		{
			slot.gameObject.SetActive(false);
		}

		int invItemCount = 0;
		for (int i = 0; i < InventoryMngr.AvailableItemSlots; i++)
		{
			int itemIndex = 0;
			ItemIcons[i].gameObject.SetActive(true);
			foreach (GameObject invItem in InventoryMngr.CollectedItems.Keys)
			{
				// Enabling active slots and setting item images
				if (itemIndex == invItemCount)
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

			// Set selected icon colour
			if(i == InventoryMngr.SelectedInvSlot)
            {
                ItemIcons[i].BackgroundImage.color = SelectedItemColor;
            }
            else
            {
                ItemIcons[i].BackgroundImage.color = UnselectedItemColor;
            }
        }
	}
}
