using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

/// <summary>
/// Class that manages each row of skills in the skills UI window
/// </summary>

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
}
