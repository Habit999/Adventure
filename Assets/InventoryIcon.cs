using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryIcon : MonoBehaviour
{
	public enum ICONTYPE { ItemSlot, HotbarButton };
	[SerializeField] ICONTYPE IconType;

	public InventoryUI InventoryUIController;

	[SerializeField] private Image itemImage;
    public Image BackgroundImage;

	[SerializeField] private TextMeshProUGUI itemAmount;

    private void Awake()
    {
		if(itemImage != null)
		{
            itemImage.gameObject.SetActive(false);
			itemAmount.gameObject.SetActive(false);
        }
    }

    public void IconClick()
	{
		switch(IconType)
		{
			case ICONTYPE.ItemSlot:
				InventoryUIController.SlotClicked(transform.GetSiblingIndex());
                break;
			
			case ICONTYPE.HotbarButton:
                if (InventoryUIController.InventoryMngr.SelectedInvSlot >= 0 && InventoryUIController.InventoryMngr.SelectedInvSlot < InventoryUIController.InventoryMngr.CollectedItems.Count)
					InventoryUIController.InventoryMngr.AssignHotbarItem(transform.GetSiblingIndex());
				break;
			
			default:
				break;
		}
	}

	public void UpdateItemImage(Sprite image)
	{
		if (image != null)
		{
			itemImage.sprite = image;
			itemImage.gameObject.SetActive(true);
        }
		else
		{
			itemImage.gameObject.SetActive(false);
        }
    }

	public void UpdateItemAmount(int amount)
	{
		if (amount <= 0)
		{
			itemAmount.gameObject.SetActive(false);
		}
		else
		{
			itemAmount.text = amount.ToString();
			itemAmount.gameObject.SetActive(true);
		}
	}
}
