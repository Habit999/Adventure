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
	
	void Update()
	{
		objectPresent = Physics.Raycast(Controller._camera.position, Controller._camera.forward, out playerCamHit, _interactionDistance);
	}
	
	public INTERACTIONOUTCOMES Interact()
	{
		if(objectPresent)
		{
			if(playerCamHit.collider.gameObject.tag == "Map")
			{
				Controller.FreezePlayer();
				playerCamHit.collider.gameObject.GetComponent<LevelMap>().OpenMap();
				return INTERACTIONOUTCOMES.Successful;
			}
			else return INTERACTIONOUTCOMES.None;
		}
		else return INTERACTIONOUTCOMES.None;
	}
	
	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(Controller._camera.position, Controller._camera.position + Controller._camera.forward * _interactionDistance);
	}
	#endif
}
