using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceMediator : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private UserInterfaceController userInterfaceController;

    private void OnEnable()
    {
        playerController.InteractionMngr.OnSwitchHotbar += userInterfaceController.ChangeActiveHotbar;
    }

    private void OnDisable()
    {
        playerController.InteractionMngr.OnSwitchHotbar -= userInterfaceController.ChangeActiveHotbar;
    }
}
