using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : IInteractable
{
	public bool _isOpen;
	
	[SerializeField] GameObject itemInChest;
	
	public GameObject OpenChest()
	{
		GameObject spawnedItem = Instantiate(itemInChest, Vector3.zero, Quaternion.identity);
		spawnedItem.SetActive(false);
		_isOpen = true;
		return spawnedItem;
	}
	
	void Awake()
	{
		_isOpen = false;
	}
	
	void OnMouseDown()
	{
		if(!_isOpen) OpenChest();
	}
}
