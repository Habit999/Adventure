using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
	public enum INTERACTIONOUTCOMES { None, Successful, Failed };
	
	public PlayerController Controller { get { return gameObject.GetComponent<PlayerController>(); } }
	
	[HideInInspector] public GameObject ObjectInView;
	[HideInInspector] public bool ObjectPresent;
	
	public float InteractionDistance;
	
	// Player perspective raycast variables
	RaycastHit playerCamHit;
	Ray playerCamRay;
	
	public INTERACTIONOUTCOMES Interact()
	{
		if(ObjectPresent)
		{
			if(playerCamHit.collider.gameObject.tag == "Map")
			{
				Controller.FreezePlayer(true, true);
				playerCamHit.collider.gameObject.GetComponent<LevelMap>().OpenMap();
				return INTERACTIONOUTCOMES.Successful;
			}
			else if(playerCamHit.collider.gameObject.tag == "Chest")
			{
				playerCamHit.collider.gameObject.GetComponent<ChestSpawnPoint>().OpenChest();
				return INTERACTIONOUTCOMES.Successful;
			}
			else if(playerCamHit.collider.gameObject.tag == "Exit")
			{
				playerCamHit.collider.gameObject.GetComponent<DungeonExit>().Exit();
				return INTERACTIONOUTCOMES.Successful;
			}
			else return INTERACTIONOUTCOMES.None;
		}
		else return INTERACTIONOUTCOMES.None;
	}
	
	void Update()
	{
		ObjectPresent = Physics.Raycast(Controller.Camera.position, Controller.Camera.forward, out playerCamHit, InteractionDistance);
		if(ObjectPresent) ObjectInView = playerCamHit.collider.gameObject;
		
		HotBarInteraction();
		
		ItemUsage();
	}
	
	void ItemUsage()
	{
		if(Controller.PlayerState == PlayerController.PLAYERSTATE.FreeLook && !Controller.MouseToggled)
		{
			if(Input.GetMouseButtonDown(GameManager.Instance.Controls.MousePrimary) && Controller.InventoryMngr._equippedItem != null)
			{
				Controller.InventoryMngr._equippedItem.GetComponent<Item>().UseItem();
				Controller.CombatMngr._rightHandAnimator.SetTrigger(Controller.InventoryMngr._equippedItem.GetComponent<Item>()._animatorTriggerName);
			}
		}
	}
	
	void HotBarInteraction()
	{
		UserInterfaceController controllerUI = UserInterfaceController.Instance;
		if(Input.GetKeyDown(GameManager.Instance.Controls.HotBar0))
		{
			controllerUI.SetActiveActionKey(0);
		}
		else if(Input.GetKeyDown(GameManager.Instance.Controls.HotBar1))
		{
			controllerUI.SetActiveActionKey(1);
		}
		else if(Input.GetKeyDown(GameManager.Instance.Controls.HotBar2))
		{
			controllerUI.SetActiveActionKey(2);
		}
		else if(Input.GetKeyDown(GameManager.Instance.Controls.HotBar3))
		{
			controllerUI.SetActiveActionKey(3);
		}
		else if(Input.GetKeyDown(GameManager.Instance.Controls.HotBar4))
		{
			controllerUI.SetActiveActionKey(4);
		}
		else if(Input.GetKeyDown(GameManager.Instance.Controls.HotBar5))
		{
			controllerUI.SetActiveActionKey(5);
		}
		else if(Input.GetKeyDown(GameManager.Instance.Controls.HotBar6))
		{
			controllerUI.SetActiveActionKey(6);
		}
		else if(Input.GetKeyDown(GameManager.Instance.Controls.HotBar7))
		{
			controllerUI.SetActiveActionKey(7);
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
