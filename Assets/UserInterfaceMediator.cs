using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceMediator : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private UserInterfaceController userInterfaceController;

    private void OnEnable()
    {
        if(playerController != null)
        {
            playerController.InteractionMngr.OnSwitchHotbar += userInterfaceController.ChangeActiveHotbar;
            playerController.OnVanish += userInterfaceController.Vanished;
            playerController.OnDeath += userInterfaceController.Died;
        }
    }

    private void OnDisable()
    {
        if(playerController != null)
        {
            playerController.InteractionMngr.OnSwitchHotbar -= userInterfaceController.ChangeActiveHotbar;
            playerController.OnVanish += userInterfaceController.Vanished;
            playerController.OnDeath += userInterfaceController.Died;
        }
    }
}
