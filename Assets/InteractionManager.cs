using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
	private PlayerController controller;

	public event Action<int> OnSwitchHotbar;
	
	[HideInInspector] public GameObject ObjectInView = null;
	[HideInInspector] public bool ObjectPresent;
	
	public float InteractionDistance;
	
	// Player perspective raycast variables
	RaycastHit playerCamHit;
	Ray playerCamRay;

    private void Awake()
    {
        controller = gameObject.GetComponent<PlayerController>();
    }

    public void Interact()
	{
		if(Input.GetKeyDown(GameManager.Instance.Controls.Interact) && ObjectPresent)
		{
			if(ObjectInView.tag == "Map")
			{
				controller.FreezePlayer(true, true);
				ObjectInView.GetComponent<LevelMap>().OpenMap();
			}
			else if(ObjectInView.tag == "Chest")
			{
				ObjectInView.GetComponent<TreasureChest>().OpenChest();
			}
			else if(ObjectInView.tag == "Exit")
			{
				ObjectInView.GetComponent<DungeonExit>().Exit();
			}
		}
	}
	
	void Update()
	{
		ObjectPresent = Physics.Raycast(controller.Camera.position, controller.Camera.forward, out playerCamHit, InteractionDistance);
		if(ObjectPresent) ObjectInView = playerCamHit.collider.gameObject;

		Interact();
		
		HotBarInteraction();
		
		ItemUsage();
	}
	
	void ItemUsage()
	{
		if(controller.PlayerState == PlayerController.PLAYERSTATE.FreeLook && !controller.MouseToggled)
		{
			if(Input.GetMouseButtonDown(GameManager.Instance.Controls.MousePrimary) && controller.InventoryMngr.EquippedItem != null)
			{
				controller.InventoryMngr.EquippedItem.GetComponent<Item>().UseItem();
				controller.CombatMngr.RightHandAnimator.SetTrigger(controller.InventoryMngr.EquippedItem.GetComponent<Item>().AnimatorTriggerName);
			}
		}
	}
	
	void HotBarInteraction()
	{
		if(Input.GetKeyDown(GameManager.Instance.Controls.HotBar0))
		{
			OnSwitchHotbar?.Invoke(0);
            controller.InventoryMngr.EquipItem();
        }
		else if(Input.GetKeyDown(GameManager.Instance.Controls.HotBar1))
		{
			OnSwitchHotbar?.Invoke(1);
			controller.InventoryMngr.EquipItem();
		}
		else if(Input.GetKeyDown(GameManager.Instance.Controls.HotBar2))
		{
			OnSwitchHotbar?.Invoke(2);
			controller.InventoryMngr.EquipItem();
		}
		else if(Input.GetKeyDown(GameManager.Instance.Controls.HotBar3))
		{
			OnSwitchHotbar?.Invoke(3);
			controller.InventoryMngr.EquipItem();
		}
		else if(Input.GetKeyDown(GameManager.Instance.Controls.HotBar4))
		{
			OnSwitchHotbar?.Invoke(4);
			controller.InventoryMngr.EquipItem();
		}
		else if(Input.GetKeyDown(GameManager.Instance.Controls.HotBar5))
		{
			OnSwitchHotbar?.Invoke(5);
			controller.InventoryMngr.EquipItem();
		}
		else if(Input.GetKeyDown(GameManager.Instance.Controls.HotBar6))
		{
			OnSwitchHotbar?.Invoke(6);
			controller.InventoryMngr.EquipItem();
		}
		else if(Input.GetKeyDown(GameManager.Instance.Controls.HotBar7))
		{
			OnSwitchHotbar?.Invoke(7);
			controller.InventoryMngr.EquipItem();
		}
	}
	
	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		if(controller != null)
		{
            Gizmos.color = Color.red;
            Gizmos.DrawLine(controller.Camera.position, controller.Camera.position + controller.Camera.forward * InteractionDistance);
        }
    }
	#endif
}
