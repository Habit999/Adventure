using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryIcon : MonoBehaviour
{
	public enum ICONTYPE { ItemSlot, HotbarButton };
	[SerializeField] ICONTYPE IconType;

	[HideInInspector] public InventoryUI InventoryController;

	[SerializeField] private Image itemImage;
	private Image backgroundImage;

    private void Awake()
    {
        backgroundImage = GetComponent<Image>();

		itemImage.gameObject.SetActive(false);
    }

    public void IconClick()
	{
		switch(IconType)
		{
			case ICONTYPE.ItemSlot:
				InventoryController.SlotClicked(transform.GetSiblingIndex());
                break;
			
			case ICONTYPE.HotbarButton:
				if(PlayerController.Instance.InventoryMngr.SelectedInvSlot >= 0 && PlayerController.Instance.InventoryMngr.SelectedInvSlot < PlayerController.Instance.InventoryMngr.CollectedItems.Count)
					PlayerController.Instance.InventoryMngr.AssignHotbarItem(transform.GetSiblingIndex() + 1);
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
			backgroundImage.color = InventoryController.SelectedItemColor;
        }
		else
		{
			itemImage.gameObject.SetActive(false);
            backgroundImage.color = InventoryController.UnselectedItemColor;
        }
    }
}
