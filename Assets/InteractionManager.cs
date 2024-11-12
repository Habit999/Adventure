using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
	public enum INTERACTIONOUTCOMES { None, Successful, Failed };
	
	public PlayerController Controller { get { return gameObject.GetComponent<PlayerController>(); } }
	
	[HideInInspector] public GameObject _objectInView;
	
	public float _interactionDistance;
	
	// Player perspective raycast variables
	RaycastHit playerCamHit;
	Ray playerCamRay;
	bool objectPresent;
	
	public INTERACTIONOUTCOMES Interact()
	{
		if(objectPresent)
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
			else return INTERACTIONOUTCOMES.None;
		}
		else return INTERACTIONOUTCOMES.None;
	}
	
	void Update()
	{
		objectPresent = Physics.Raycast(Controller._camera.position, Controller._camera.forward, out playerCamHit, _interactionDistance);
		
		HotBarInteraction();
	}
	
	void HotBarInteraction()
	{
		UserInterfaceController controllerUI = UserInterfaceController.Instance;
		if(Input.GetKeyDown(Controls.HotBar0)) controllerUI.SetActiveActionKey(0);
		else if(Input.GetKeyDown(Controls.HotBar1)) controllerUI.SetActiveActionKey(1);
		else if(Input.GetKeyDown(Controls.HotBar2)) controllerUI.SetActiveActionKey(2);
		else if(Input.GetKeyDown(Controls.HotBar3)) controllerUI.SetActiveActionKey(3);
		else if(Input.GetKeyDown(Controls.HotBar4)) controllerUI.SetActiveActionKey(4);
		else if(Input.GetKeyDown(Controls.HotBar5)) controllerUI.SetActiveActionKey(5);
		else if(Input.GetKeyDown(Controls.HotBar6)) controllerUI.SetActiveActionKey(6);
		else if(Input.GetKeyDown(Controls.HotBar7)) controllerUI.SetActiveActionKey(7);
	}
	
	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(Controller._camera.position, Controller._camera.position + Controller._camera.forward * _interactionDistance);
	}
	#endif
}
