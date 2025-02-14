using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
	public PlayerController Controller { get { return gameObject.GetComponent<PlayerController>(); } }

	public event Action<int> OnSwitchHotbar;
	
	[HideInInspector] public GameObject ObjectInView = null;
	[HideInInspector] public bool ObjectPresent;
	
	public float InteractionDistance;
	
	// Player perspective raycast variables
	RaycastHit playerCamHit;
	Ray playerCamRay;
	
	public void Interact()
	{
		if(Input.GetKeyDown(GameManager.Instance.Controls.Interact) && ObjectPresent)
		{
			if(ObjectInView.tag == "Map")
			{
				Controller.FreezePlayer(true, true);
				ObjectInView.GetComponent<LevelMap>().OpenMap();
			}
			else if(ObjectInView.tag == "Chest")
			{
				//ObjectInView.GetComponent<ChestSpawnPoint>().OpenChest();
			}
			else if(ObjectInView.tag == "Exit")
			{
				ObjectInView.GetComponent<DungeonExit>().Exit();
			}
		}
	}
	
	void Update()
	{
		ObjectPresent = Physics.Raycast(Controller.Camera.position, Controller.Camera.forward, out playerCamHit, InteractionDistance);
		if(ObjectPresent) ObjectInView = playerCamHit.collider.gameObject;

		Interact();
		
		HotBarInteraction();
		
		ItemUsage();
	}
	
	void ItemUsage()
	{
		if(Controller.PlayerState == PlayerController.PLAYERSTATE.FreeLook && !Controller.MouseToggled)
		{
			if(Input.GetMouseButtonDown(GameManager.Instance.Controls.MousePrimary) && Controller.InventoryMngr.EquippedItem != null)
			{
				Controller.InventoryMngr.EquippedItem.GetComponent<Item>().UseItem();
				Controller.CombatMngr._rightHandAnimator.SetTrigger(Controller.InventoryMngr.EquippedItem.GetComponent<Item>().AnimatorTriggerName);
			}
		}
	}
	
	void HotBarInteraction()
	{
		if(Input.GetKeyDown(GameManager.Instance.Controls.HotBar0))
		{
			OnSwitchHotbar?.Invoke(0);
            Controller.InventoryMngr.EquipItem();
        }
		else if(Input.GetKeyDown(GameManager.Instance.Controls.HotBar1))
		{
			OnSwitchHotbar?.Invoke(1);
			Controller.InventoryMngr.EquipItem();
		}
		else if(Input.GetKeyDown(GameManager.Instance.Controls.HotBar2))
		{
			OnSwitchHotbar?.Invoke(2);
			Controller.InventoryMngr.EquipItem();
		}
		else if(Input.GetKeyDown(GameManager.Instance.Controls.HotBar3))
		{
			OnSwitchHotbar?.Invoke(3);
			Controller.InventoryMngr.EquipItem();
		}
		else if(Input.GetKeyDown(GameManager.Instance.Controls.HotBar4))
		{
			OnSwitchHotbar?.Invoke(4);
			Controller.InventoryMngr.EquipItem();
		}
		else if(Input.GetKeyDown(GameManager.Instance.Controls.HotBar5))
		{
			OnSwitchHotbar?.Invoke(5);
			Controller.InventoryMngr.EquipItem();
		}
		else if(Input.GetKeyDown(GameManager.Instance.Controls.HotBar6))
		{
			OnSwitchHotbar?.Invoke(6);
			Controller.InventoryMngr.EquipItem();
		}
		else if(Input.GetKeyDown(GameManager.Instance.Controls.HotBar7))
		{
			OnSwitchHotbar?.Invoke(7);
			Controller.InventoryMngr.EquipItem();
		}
	}
	
	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(Controller.Camera.position, Controller.Camera.position + Controller.Camera.forward * InteractionDistance);
	}
	#endif
}
