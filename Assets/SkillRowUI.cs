using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillRowUI : MonoBehaviour
{
	[HideInInspector] public int _skillValue;
	
	[HideInInspector] public bool _canIncrease;
	[HideInInspector] public bool _canDecrease;
	
	[SerializeField] SkillsManager.SKILLTYPE skillType;
	[Space(5)]
	[SerializeField] TextMeshProUGUI valueText;
	[SerializeField] Button plusButton;
	[SerializeField] Button minusButton;
	
	public void IncreaseValueButton()
	{
		PlayerController.Instance.SkillsMngr.ChangeSkillValue(skillType, 1);
	}
	
	public void DecreaseValueButton()
	{
		PlayerController.Instance.SkillsMngr.ChangeSkillValue(skillType, -1);
	}
	
	void Update()
	{
		valueText.SetText(_skillValue.ToString());
		
		CheckSkillPoints();
		
		plusButton.interactable = _canIncrease;
		minusButton.interactable = _canDecrease;
	}
	
	void CheckSkillPoints()
	{
		if(PlayerController.Instance.SkillsMngr._tempValuesActive)
		{
			// Enables increase button if a temp skill point is available
			if(PlayerController.Instance.SkillsMngr._tempSkillPoints > 0)
				_canIncrease = true;
			else _canIncrease = false;
			
			// Enables decrease button depending on if the value has been increased or not
			// also adjusts to the skill type
			switch(skillType)
			{
				case SkillsManager.SKILLTYPE.Vitality:
					if(PlayerController.Instance.SkillsMngr._tempSkills.vitality > PlayerController.Instance.SkillsMngr._currentSkills.vitality)
						_canDecrease = true;
					else _canDecrease = false;
					break;
					
				case SkillsManager.SKILLTYPE.Strength:
					if(PlayerController.Instance.SkillsMngr._tempSkills.strength > PlayerController.Instance.SkillsMngr._currentSkills.strength)
						_canDecrease = true;
					else _canDecrease = false;
					break;
					
				case SkillsManager.SKILLTYPE.Intelligence:
					if(PlayerController.Instance.SkillsMngr._tempSkills.intelligence > PlayerController.Instance.SkillsMngr._currentSkills.intelligence)
						_canDecrease = true;
					else _canDecrease = false;
					break;
					
				default:
					break;
			}
		}
		else 
		{
			// Enables increase button if an actual skill point is available
			if(PlayerController.Instance.SkillsMngr._skillPoints > 0)
				_canIncrease = true;
			else _canIncrease = false;
			
			_canDecrease = false;
		}
	}
}
