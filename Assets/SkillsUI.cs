using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillsUI : MonoBehaviour
{
    [HideInInspector] public SkillsManager SkillsMngr;

    [SerializeField] private Transform skillsGroup;
    [SerializeField] private Button confirmButton;
    [SerializeField] private TextMeshProUGUI skillPointDisplay;
    [SerializeField] private TextMeshProUGUI playerLevelDisplay;

    public event Action OnSkillPointsChanged;

    private List<SkillRowUI> skillRowUIs = new List<SkillRowUI>();

    private SkillsManager.SkillsList tempSkillsList = new SkillsManager.SkillsList(1, 1);

    private int tempSkillPoints;

    private Animator animator;

    private bool isOpen;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        foreach (var skillRow in skillsGroup.GetComponentsInChildren<SkillRowUI>())
        {
            skillRowUIs.Add(skillRow);
            skillRow.SkillsUIController = this;
        }
    }

    private void Start()
    {
        ResetSkills();
    }

    private void Update()
    {
        if (SkillsMngr.Controller.PlayerState != PlayerController.PLAYERSTATE.InMenu && isOpen)
        {
            ToggleOpen();
            ResetSkills();
        }
    }


    private void ResetSkills()
    {
        tempSkillsList.vitality = SkillsMngr.CurrentSkills.vitality;
        tempSkillsList.strength = SkillsMngr.CurrentSkills.strength;
        tempSkillPoints = SkillsMngr.SkillPoints;
        UpdateSkillsWindow();
    }

    public void ToggleOpen()
    {
        isOpen = !isOpen;
        if (isOpen)
            ResetSkills();
        animator.SetBool("Open", isOpen);
    }

    public void ConfirmSkillChanges()
    {
        SkillsMngr.CurrentSkills.vitality = tempSkillsList.vitality;
        SkillsMngr.CurrentSkills.strength = tempSkillsList.strength;
        SkillsMngr.SkillPoints = tempSkillPoints;
        UpdateSkillsWindow();
    }

    private void UpdateSkillsWindow()
    {
        foreach (var skillRow in skillRowUIs)
        {
            // Check if value can be decreased and set the value
            switch (skillRow.SkillType)
            {
                case SkillsManager.SKILLTYPE.Vitality:
                    if (tempSkillsList.vitality > SkillsMngr.CurrentSkills.vitality)
                        skillRow.CanDecrease = true;
                    else skillRow.CanDecrease = false;
                    skillRow.SkillValue = tempSkillsList.vitality;
                    break;

                case SkillsManager.SKILLTYPE.Strength:
                    if (tempSkillsList.strength > SkillsMngr.CurrentSkills.strength)
                        skillRow.CanDecrease = true;
                    else skillRow.CanDecrease = false;
                    skillRow.SkillValue = tempSkillsList.strength;
                    break;

                default:
                    break;
            }

            // Check if value can be increased
            if (tempSkillPoints > 0)
                skillRow.CanIncrease = true;
            else skillRow.CanIncrease = false;
        }

        // Set display
        if(tempSkillsList.vitality != SkillsMngr.CurrentSkills.vitality || tempSkillsList.strength != SkillsMngr.CurrentSkills.strength) 
            confirmButton.interactable = true;
        else confirmButton.interactable = false;
        skillPointDisplay.SetText("Points: " + tempSkillPoints.ToString());
        playerLevelDisplay.SetText(SkillsMngr.PlayerLevel.ToString());

        OnSkillPointsChanged?.Invoke();
    }

    public void ChangeTempSkillValue(SkillsManager.SKILLTYPE skillType, int amount)
    {
        switch (skillType)
        {
            case SkillsManager.SKILLTYPE.Vitality:
                tempSkillsList.vitality += amount;
                break;

            case SkillsManager.SKILLTYPE.Strength:
                tempSkillsList.strength += amount;
                break;

            default:
                break;
        }

        tempSkillPoints -= amount;

        UpdateSkillsWindow();
    }
}
