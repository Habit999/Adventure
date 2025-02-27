using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceMediator : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private UserInterfaceController userInterfaceController;
    [SerializeField] private InventoryUI inventoryUI;

    private void Awake()
    {
        inventoryUI.InvManager = playerController.InventoryMngr;
    }

    private void OnEnable()
    {
        playerController.InteractionMngr.OnSwitchHotbar += userInterfaceController.ChangeActiveHotbar;
        playerController.OnVanish += userInterfaceController.Vanished;
        playerController.OnDeath += userInterfaceController.Died;
    }

    private void OnDisable()
    {
        playerController.InteractionMngr.OnSwitchHotbar -= userInterfaceController.ChangeActiveHotbar;
        playerController.OnVanish += userInterfaceController.Vanished;
        playerController.OnDeath += userInterfaceController.Died;
    }
}
