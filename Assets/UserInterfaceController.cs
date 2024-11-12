using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInterfaceController : MonoBehaviour
{
	public static UserInterfaceController Instance;
	
	protected int ItemCountInInventory { get { return PlayerController.Instance.InventoryMngr._collectedItems.Count; } }
	protected int MaxActionKeysInRow { get { return actionKeys.childCount; } }
	protected int MaxActionPages { get { return (ItemCountInInventory / MaxActionKeysInRow) + ((ItemCountInInventory % MaxActionKeysInRow > 0) ? 1 : 0); } }
	
	public ProgressBar _healthBar;
	public ProgressBar _manaBar;
	
	[HideInInspector] public int currentActiveActionKey;
	[Space(10)]
	[SerializeField] Transform actionKeys;
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
	[SerializeField] Transform actionsKeys;
	[SerializeField] Color inactiveKeyColor;
	[SerializeField] Color activeKeyColor;
	[Space(10)]
	[SerializeField] List<GameObject> lockedMovementUI = new List<GameObject>();
	
	int actionKeyPage;
	
	bool isInvOpen;
	bool isSkillsOpen;
	
	void Awake()
	{
		if(Instance == null) Instance = this;
		else Destroy(this.gameObject);
		
		// Setting default values
		actionKeyPage = 1;
		
		currentActiveActionKey = 0;
		
		skillsAnimator = skillsUI.GetComponent<Animator>();
		
		playerLvlMsg = skillsUI.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
		skillPointsMsg = skillsUI.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
		skillsContent = skillsUI.GetChild(0).GetChild(2);
		confirmButton = skillsUI.GetChild(0).GetChild(3).gameObject;
		
		isInvOpen = false;
		isSkillsOpen = false;
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
		
		// Set action keys page number
		pageNumberText.SetText(actionKeyPage.ToString());
	}
	
	void CheckPlayerState()
	{
		PlayerController player = PlayerController.Instance;
		
		// Setting UI health & mana bar
		_healthBar._value = player._health / player._maxHealth;
		_manaBar._value = player._mana / player._maxMana;
		
		// Toggles turning UI buttons depending on player state
		if(player.PlayerState == PlayerController.PLAYERSTATE.FreeMove)
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
	
	#region UI Buttons
	
	// Hotbar Menu Buttons
	public void ToggleInventory()
	{
		// Close other menu's if they're open
		if(isSkillsOpen) ToggleSkills();
		
		// Toggle animator
		isInvOpen = !isInvOpen;
		inventoryUI._invAnimator.SetBool("Open", isInvOpen);
		
		PlayerController.Instance.InventoryMngr._selectedInvSlot = -1;
	}
	
	public void ToggleSkills()
	{
		// Close other menu's if they're open
		if(isInvOpen) ToggleInventory();
		
		// Disable temp skill values
		PlayerController.Instance.SkillsMngr._tempValuesActive = false;
		
		// Toggle animator
		isSkillsOpen = !isSkillsOpen;
		skillsAnimator.SetBool("Open", isSkillsOpen);
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
		if(currentActiveActionKey != 0) 
			actionsKeys.GetChild(currentActiveActionKey - 1).gameObject.GetComponent<RawImage>().color = inactiveKeyColor;
		
		// Return if 0 selected
		if(selectedActionKey == 0)
		{
			currentActiveActionKey = selectedActionKey;
			return;
		}
		
		// Activate selected if not 0
		actionsKeys.GetChild(selectedActionKey - 1).gameObject.GetComponent<RawImage>().color = activeKeyColor;
		currentActiveActionKey = selectedActionKey;
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
