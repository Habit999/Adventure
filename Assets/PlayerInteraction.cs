using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles player interactions
/// </summary>

public class PlayerInteraction : MonoBehaviour
{
	//References
	private PlayerController controller;

	public event Action<int> OnSwitchHotbar;

    RaycastHit playerCamHit;

    [HideInInspector] public GameObject ObjectInView = null;

	// Variables
	[HideInInspector] public bool ObjectPresent;
	
	public float InteractionDistance;
	
    private void Awake()
    {
        controller = gameObject.GetComponent<PlayerController>();
    }

    private void OnDisable()
    {
        OnSwitchHotbar = null;
    }

	void Update()
	{
		ObjectPresent = Physics.Raycast(controller.Camera.position, controller.Camera.forward, out playerCamHit, InteractionDistance);
		if (ObjectPresent) ObjectInView = playerCamHit.collider.gameObject;

        Interact();

        HotBarInteraction();
		
		ItemUsage();
    }

    private void Interact()
    {
        if (Input.GetKeyDown(controller.InputControls.Interact) && ObjectPresent)
        {
            if (ObjectInView.tag == "Map")
            {
                controller.FreezePlayer(true, true);
                ObjectInView.GetComponent<LevelMap>().OpenMap(controller);
            }

            if (ObjectInView.tag == "Chest")
            {
                ObjectInView.GetComponent<TreasureChest>().OpenChest(controller);
            }

            if (ObjectInView.tag == "Exit")
            {
                ObjectInView.GetComponent<DungeonExit>().Exit();
            }
        }
    }

    private void ItemUsage()
	{
		if(controller.PlayerState == PlayerController.PLAYERSTATE.FreeLook && !controller.MouseToggled)
		{
			if(Input.GetMouseButtonDown(controller.InputControls.MousePrimary) && controller.InventoryMngr.EquippedItem != null && !controller.CombatMngr.IsAnimating)
			{
                controller.InventoryMngr.EquippedItem.GetComponent<Item>().UseItem();
            }
        }
	}
	
	// Switches between hotbar slots
	private void HotBarInteraction()
	{
		if(Input.GetKeyDown(controller.InputControls.HotBar1))
		{
			OnSwitchHotbar?.Invoke(1);
			controller.InventoryMngr.EquipItem();
		}
		else if(Input.GetKeyDown(controller.InputControls.HotBar2))
		{
			OnSwitchHotbar?.Invoke(2);
			controller.InventoryMngr.EquipItem();
		}
		else if(Input.GetKeyDown(controller.InputControls.HotBar3))
		{
			OnSwitchHotbar?.Invoke(3);
			controller.InventoryMngr.EquipItem();
		}
		else if(Input.GetKeyDown(controller.InputControls.HotBar4))
		{
			OnSwitchHotbar?.Invoke(4);
			controller.InventoryMngr.EquipItem();
		}
		else if(Input.GetKeyDown(controller.InputControls.HotBar5))
		{
			OnSwitchHotbar?.Invoke(5);
			controller.InventoryMngr.EquipItem();
		}
		else if(Input.GetKeyDown(controller.InputControls.HotBar6))
		{
			OnSwitchHotbar?.Invoke(6);
			controller.InventoryMngr.EquipItem();
		}
		else if(Input.GetKeyDown(controller.InputControls.HotBar7))
		{
			OnSwitchHotbar?.Invoke(7);
			controller.InventoryMngr.EquipItem();
		}
	}
	
	#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		if(controller != null)
		{
            Gizmos.color = Color.red;
            Gizmos.DrawLine(controller.Camera.position, controller.Camera.position + controller.Camera.forward * InteractionDistance);
        }
    }
	#endif
}
