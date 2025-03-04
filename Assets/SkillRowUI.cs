using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class SkillRowUI : MonoBehaviour
{
	[HideInInspector] public SkillsUI SkillsUIController;

    [HideInInspector] public int SkillValue;
	
	[HideInInspector] public bool CanIncrease;
	[HideInInspector] public bool CanDecrease;
	
	public SkillsManager.SKILLTYPE SkillType;
	[Space(5)]
	[SerializeField] private TextMeshProUGUI valueText;
	[SerializeField] private Button plusButton;
	[SerializeField] private Button minusButton;

    private void Start()
    {
		SkillsUIController.OnSkillPointsChanged += SetRowDisplay;
    }

    private void OnDisable()
    {
        SkillsUIController.OnSkillPointsChanged -= SetRowDisplay;
    }

    public void IncreaseValueButton()
	{
		SkillsUIController.ChangeTempSkillValue(SkillType, 1);
    }

    public void DecreaseValueButton()
	{
        SkillsUIController.ChangeTempSkillValue(SkillType, -1);
    }

    private void SetRowDisplay()
	{
        valueText.SetText(SkillValue.ToString());
        plusButton.interactable = CanIncrease;
        minusButton.interactable = CanDecrease;
    }

    /*private void CheckSkillPoints()
	{
		if(SkillsUIController.SkillsMngr.TempValuesActive)
		{
			// Enables increase button if a temp skill point is available
			if(SkillsUIController.SkillsMngr.TempSkillPoints > 0)
				CanIncrease = true;
			else CanIncrease = false;
			
			// Enables decrease button depending on if the value has been increased or not
			// also adjusts to the skill type
			switch(SkillType)
			{
				case SkillsManager.SKILLTYPE.Vitality:
					if(SkillsUIController.SkillsMngr.TempSkills.vitality > SkillsUIController.SkillsMngr.CurrentSkills.vitality)
						CanDecrease = true;
					else CanDecrease = false;
					break;
					
				case SkillsManager.SKILLTYPE.Strength:
					if(SkillsUIController.SkillsMngr.TempSkills.strength > SkillsUIController.SkillsMngr.CurrentSkills.strength)
						CanDecrease = true;
					else CanDecrease = false;
					break;
					
				default:
					break;
			}
		}
		else 
		{
			// Enables increase button if an actual skill point is available
			if(SkillsUIController.SkillsMngr.SkillPoints > 0)
				CanIncrease = true;
			else CanIncrease = false;
			
			CanDecrease = false;
		}

        plusButton.interactable = CanIncrease;
        minusButton.interactable = CanDecrease;
    }*/
}
