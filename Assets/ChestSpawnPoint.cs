using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawnPoint : IInteractable
{	
	/*[HideInInspector] */public bool _willSpawn = false;
	
	public bool _isOpen;
	
	GameObject chest;
	
	GameObject itemInChest;
	
	Animator chestAnimator;
	
	public bool chestSpawned = false;
	
	public void OpenChest()
	{
		if(!_canInteract || itemInChest == null) return;
		print("FuckThis");
		bool collectedItem = itemInChest.GetComponent<U_Item>().CollectItem();
		print(collectedItem);
		if(collectedItem)
		{
			_isOpen = true;
			chestAnimator.SetBool("Open", true);
		}
		_canInteract = false;
	}
	
	void Awake()
	{
		_willSpawn = false;
		
		chest = transform.GetChild(0).gameObject;
		chest.SetActive(false);
		
		chestAnimator = transform.GetChild(0).gameObject.GetComponent<Animator>();
		
		chestSpawned = false;
		_isOpen = false;
	}
	
	void Update()
	{
		if(_willSpawn && !chestSpawned) SpawnChest();
	}
	
	void SpawnChest()
	{
		print("Chest Spawned");
		chestSpawned = true;
		chest.SetActive(true);
		itemInChest = SpawnManager.Instance.GenerateRandomLoot(SpawnManager.RANDOMLOOTTYPE.Any);
		print(itemInChest);
	}
	
	void OnMouseDown()
	{
		if(!_isOpen && _canInteract) OpenChest();
	}
	
	#if UNITY_EDITOR
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Transform chestModel = transform.GetChild(0);
		Gizmos.DrawLine(chestModel.position, chestModel.position + -chestModel.right);
	}
	
	#endif
}
