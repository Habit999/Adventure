using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint : MonoBehaviour
{
	public Transform _cameraTarget;
	public Transform _orientation;
	[Space(5)]
	[SerializeField] GameObject movePointIcon;
	[Space(5)]
	[Tooltip("A list of objects the player can interact with from this spot")]
	[SerializeField] List<GameObject> interactableObjects;
	
	public void SetInteracables(bool active)
	{
		foreach(GameObject interactable in interactableObjects)
		{
			if(interactable.GetComponent<IInteractable>() != null)
				interactable.GetComponent<IInteractable>()._canInteract = active;
			else print("ERROR => Non-Interactable Object In InteractableObjects List!");
		}
	}
	
	void Start()
	{
		movePointIcon.SetActive(false);
	}
	
	void OnMouseOver()
	{
		movePointIcon.SetActive(true);
		
		movePointIcon.transform.LookAt(PlayerController.Instance.Camera.transform, Vector3.up);
	}
	
	void OnMouseExit()
	{
		movePointIcon.SetActive(false);
	}
}
