using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInterfaceController : MonoBehaviour
{
	public static UserInterfaceController Instance;
	
	public int MaxActionKeysInRow { get { return actionKeys.childCount; } }
	protected int ItemCountInInventory { get { return PlayerController.Instance.InventoryMngr._collectedItems.Count; } }
	protected int MaxActionPages { get { return (ItemCountInInventory / MaxActionKeysInRow) + ((ItemCountInInventory % MaxActionKeysInRow > 0) ? 1 : 0); } }
	
	public ProgressBar HealthBar;
	public ProgressBar ManaBar;
	public ProgressBar ExperienceBar;
	
	[HideInInspector] public int CurrentActiveActionKey;
	[Space(10)]
	[SerializeField] Transform actionKeys;
	[SerializeField] Color inactiveKeyColor;
	[SerializeField] Color activeKeyColor;
	[SerializeField] TextMeshProUGUI pageNumberText;
	[Space(10)]
	[SerializeField] InventoryUI inventoryUI;
	[SerializeField] Transform skillsUI;
	Animator skillsAnimator;
	TextMeshProUGUI playerLvlMsg;
	TextMeshProUGUI skillPointsMsg;
	Transform skillsContent;
	GameObject confirmButton;
	[Space(10)]
	[SerializeField] List<GameObject> lockedMovementUI = new List<GameObject>();
	
	int actionKeyPage;
	
	[HideInInspector] public bool IsInvOpen;
	[HideInInspector] public bool IsSkillsOpen;
	
	void Awake()
	{
		if(Instance == null) Instance = this;
		else Destroy(this.gameObject);
		
		// Setting default values
		actionKeyPage = 1;
		
		CurrentActiveActionKey = 0;
		
		skillsAnimator = skillsUI.GetComponent<Animator>();
		
		playerLvlMsg = skillsUI.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
		skillPointsMsg = skillsUI.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
		skillsContent = skillsUI.GetChild(0).GetChild(2);
		confirmButton = skillsUI.GetChild(0).GetChild(3).gameObject;
		
		IsInvOpen = false;
		IsSkillsOpen = false;
	}
	
	void Update()
	{
		UpdateUI();
	}
	
	void UpdateUI()
	{
		// Choose displayed & update UI based on player state
		CheckPlayerState();
		// Updates skills UI elements in skills window
		UpdateSkillsUI();
		// Update action keys with assigned inventory items
		UpdateActionKeys();
		
		// Set action keys page number
		pageNumberText.SetText(actionKeyPage.ToString());
	}
	
	void CheckPlayerState()
	{
		PlayerController player = PlayerController.Instance;
		
		// Setting UI health & mana bar
		HealthBar.Value = player.Health / player.MaxHealth;
		ManaBar.Value = player.Mana / player.MaxMana;
		ExperienceBar.Value = player.SkillsMngr._experienceGained / player.SkillsMngr._nextLevelExperience;
		
		// Toggles turning UI buttons depending on player state
		if(player.PlayerState == PlayerController.PLAYERSTATE.FreeLook)
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
		
	void UpdateSkillsUI()
	{
		PlayerController player = PlayerController.Instance;
		
		// Set player level display
		playerLvlMsg.SetText("Level: " + player.SkillsMngr._playerLevel.ToString());
		
		int skillPoints;
		bool doSkillValuesMatch = true;
		SkillsManager.SkillsList skillList;
		// Set display values as temps if they're active
		if(player.SkillsMngr._tempValuesActive)
		{
			skillPoints = player.SkillsMngr._tempSkillPoints;
			
			if(player.SkillsMngr._currentSkills.vitality != player.SkillsMngr._tempSkills.vitality) doSkillValuesMatch = false;
			if(player.SkillsMngr._currentSkills.strength != player.SkillsMngr._tempSkills.strength) doSkillValuesMatch = false;
			if(player.SkillsMngr._currentSkills.intelligence != player.SkillsMngr._tempSkills.intelligence) doSkillValuesMatch = false;
			
			skillList = player.SkillsMngr._tempSkills;
		}
		// Set display values as actuals if temps are inactive
		else
		{
			skillPoints = player.SkillsMngr._skillPoints;
			
			doSkillValuesMatch = true;
			
			skillList = player.SkillsMngr._currentSkills;
		}
		
		// Applying decisions
		skillPointsMsg.SetText("Skill Points: " + skillPoints.ToString());
		
		confirmButton.SetActive(!doSkillValuesMatch);

		skillsContent.GetChild(0).gameObject.GetComponent<SkillRowUI>()._skillValue = skillList.vitality;
		skillsContent.GetChild(1).gameObject.GetComponent<SkillRowUI>()._skillValue = skillList.strength;
		skillsContent.GetChild(2).gameObject.GetComponent<SkillRowUI>()._skillValue = skillList.intelligence;
	}
	
	void UpdateActionKeys()
	{
		// Disable images before setting the active ones
		foreach(Transform key in actionKeys)
		{
			key.GetChild(0).gameObject.SetActive(false);
		}
		
		// Setting active action keys
		foreach(GameObject hotbarItem in inventoryUI.InvManager._hotbarItemOrder.Keys)
		{
			actionKeys.GetChild(inventoryUI.InvManager._hotbarItemOrder[hotbarItem]).GetChild(0).gameObject.SetActive(true);
			actionKeys.GetChild(inventoryUI.InvManager._hotbarItemOrder[hotbarItem]).GetChild(0).gameObject.GetComponent<Image>().sprite = hotbarItem.GetComponent<Item>()._itemData.Image;
		}
	}
	
	#region UI Buttons
	
	// Hotbar Menu Buttons
	public void ToggleInventory()
	{
		// Close other menu's if they're open
		if(IsSkillsOpen) ToggleSkills();
		
		// Toggle animator
		IsInvOpen = !IsInvOpen;
		inventoryUI._invAnimator.SetBool("Open", IsInvOpen);
		
		PlayerController.Instance.InventoryMngr._selectedInvSlot = -1;
	}
	
	public void ToggleSkills()
	{
		// Close other menu's if they're open
		if(IsInvOpen) ToggleInventory();
		
		// Disable temp skill values
		PlayerController.Instance.SkillsMngr._tempValuesActive = false;
		
		// Toggle animator
		IsSkillsOpen = !IsSkillsOpen;
		skillsAnimator.SetBool("Open", IsSkillsOpen);
	}
	
	// Skills Window Button
	public void ConfirmSkillsButton()
	{
		PlayerController.Instance.SkillsMngr.ConfirmSkillChanges();
	}
	
	// Action Key Navigation
	public void SetActiveActionKey(int selectedActionKey)
	{
		// Deactivate last selected
		if(CurrentActiveActionKey != 0) 
			actionKeys.GetChild(CurrentActiveActionKey - 1).gameObject.GetComponent<Image>().color = inactiveKeyColor;
		
		// Return if 0 selected
		if(selectedActionKey == 0)
		{
			CurrentActiveActionKey = selectedActionKey;
			PlayerController.Instance.InventoryMngr.EquipItemRight();
			return;
		}
		
		// Activate selected if not 0
		actionKeys.GetChild(selectedActionKey - 1).gameObject.GetComponent<Image>().color = activeKeyColor;
		CurrentActiveActionKey = selectedActionKey;
		
		PlayerController.Instance.InventoryMngr.EquipItemRight();
	}
	
	public void DropItem()
	{
		int index = 0;
		foreach(GameObject invItem in PlayerController.Instance.InventoryMngr._collectedItems.Keys)
		{
			if(index == PlayerController.Instance.InventoryMngr._selectedInvSlot)
			{
				PlayerController.Instance.InventoryMngr.RemoveItem(invItem, 1);
				break;
			}
				
			index++;
		}
		
		PlayerController.Instance.InventoryMngr._selectedInvSlot = -1;
	}
	
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
		int itemIndex = actionIndex + (MaxActionKeysInRow * (actionKeyPage - 1));
	}
	
	#endregion
}
