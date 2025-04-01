using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceMediator : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private UserInterfaceController userInterfaceController;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private SkillsUI skillsUI;

    private void Awake()
    {
        inventoryUI.InventoryMngr = playerController.InventoryMngr;
        userInterfaceController.InventoryMngr = playerController.InventoryMngr;
        skillsUI.SkillsMngr = playerController.SkillsMngr;
    }

    private void OnEnable()
    {
        playerController.InventoryMngr.OnHotbarChange += userInterfaceController.UpdateHotbarItemImages;
        playerController.InventoryMngr.OnInventoryChange += inventoryUI.RefreshInventorySlots;
        playerController.InteractionMngr.OnSwitchHotbar += userInterfaceController.ChangeActiveHotbar;
        playerController.OnVanish += userInterfaceController.Vanished;
        playerController.OnDeath += userInterfaceController.Died;
        playerController.OnHealthChange += userInterfaceController.SetHealthBar;
        playerController.SkillsMngr.OnExperienceChange += userInterfaceController.SetExperienceBar;
        skillsUI.OnStatChange += playerController.UpdateMaxHealth;
        skillsUI.OnStatChange += playerController.InventoryMngr.UpdateInventoryStats;
    }
}
