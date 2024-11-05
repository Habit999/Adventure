using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInterfaceController : MonoBehaviour
{
	public static UserInterfaceController Instance;
	
	protected int ItemCountInInventory { get { return PlayerController.Instance.InventoryMngr._collectedItems.Count; } }
	protected int MaxActionKeys { get { return actionKeys.childCount; } }
	protected int MaxActionPages { get { return (ItemCountInInventory / MaxActionKeys) + ((ItemCountInInventory % MaxActionKeys > 0) ? 1 : 0); } }
	
	public Transform ItemIconGroup { get { return inventoryUI.GetChild(0).GetChild(0); } }
	
	[HideInInspector] public List<Transform> _itemIcons = new List<Transform>();
	
	[SerializeField] Transform actionKeys;
	[SerializeField] TextMeshProUGUI pageNumberText;
	[Space(10)]
	[SerializeField] Transform inventoryUI;
	Animator invAnimator;
	[SerializeField] Transform skillsUI;
	Animator skillsAnimator;
	[Space(10)]
	[SerializeField] List<GameObject> lockedMovementUI = new List<GameObject>();
	
	int actionKeyPage;
	
	void Awake()
	{
		if(Instance == null) Instance = this;
		else Destroy(this.gameObject);
		
		// Defaults
		actionKeyPage = 1;
		
		if(inventoryUI != null) invAnimator = inventoryUI.GetComponent<Animator>();
		if(skillsUI != null) skillsAnimator = skillsUI.GetComponent<Animator>();
		
		// Inventory Icon List
		_itemIcons.Clear();
		for(int i = 0; i < ItemIconGroup.childCount; i++)
		{
			_itemIcons.Add(ItemIconGroup.GetChild(i));
		}
	}
	
	void Update()
	{
		UpdateUI();
	}
	
	void UpdateUI()
	{
		CheckPlayerState();
		UpdateInventoryUI();
		
		// Action Keys
		pageNumberText.SetText(actionKeyPage.ToString());
	}
	
	void CheckPlayerState()
	{
		if(PlayerController.Instance.PlayerState == PlayerController.PLAYERSTATE.FreeMove)
		{
			foreach(GameObject elementUI in lockedMovementUI)
			{
				elementUI.SetActive(false);
			}
		}
		else
		{
			foreach(GameObject elementUI in lockedMovementUI)
			{
				elementUI.SetActive(true);
			}
		}
	}
	
	void UpdateInventoryUI()
	{
		// Icon Images
		foreach(Transform icon in _itemIcons)
		{
			icon.gameObject.SetActive(false);
			icon.GetChild(0).gameObject.SetActive(false);
		}
		int invItemCount = 0;
		for(int i = 0; i < PlayerController.Instance.InventoryMngr._itemSlotCount; i++)
		{
			int itemIndex = 0;
			_itemIcons[i].gameObject.SetActive(true);
			foreach(GameObject invItem in PlayerController.Instance.InventoryMngr._collectedItems.Keys)
			{
				if(itemIndex == invItemCount)
				{
					_itemIcons[i].GetChild(0).gameObject.SetActive(true);
					_itemIcons[i].GetChild(0).gameObject.GetComponent<RawImage>().texture = invItem.GetComponent<A_Item>()._itemData.Image;
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
	
	#region UI Buttons
	
	// Menu Buttons
	public void ToggleInventory()
	{
		invAnimator.SetBool("Open", !invAnimator.GetBool("Open"));
	}
	
	public void ToggleSkills()
	{
		skillsAnimator.SetBool("Open", !skillsAnimator.GetBool("Open"));
	}
	
	// Action Keys
	public void ActionKeysUp()
	{
		if(actionKeyPage - 1 > 0)
		{
			actionKeyPage--;
		}
	}
	
	public void ActionKeysDown()
	{
		if(actionKeyPage + 1 <= MaxActionPages)
		{
			actionKeyPage++;
		}
	}
	
	public void ActivateActionKey(int actionIndex)
	{
		int itemIndex = actionIndex + (MaxActionKeys * (actionKeyPage - 1));
	}
	
	// Rotation
	public void TurnRightButton()
	{
		PlayerController.Instance.RotateRight();
	}
	
	public void TurnLeftButton()
	{
		PlayerController.Instance.RotateLeft();
	}
	
	#endregion
}
