using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawnPoint : IInteractable
{	
	[Space(5)]
	public bool _willSpawn;
	
	[Space(5)]
	
	[SerializeField] GameObject interactMsg;
	
	[HideInInspector] public bool _isOpen;
	
	GameObject chest;
	
	GameObject itemInChest;
	
	Animator chestAnimator;
	BoxCollider chestCollider;
	
	
	public void OpenChest()
	{
		if(!_canInteract || itemInChest == null) return;
		bool collectedItem = itemInChest.GetComponent<Item>().CollectItem();
		if(collectedItem)
		{
			_isOpen = true;
			chestAnimator.SetBool("Open", true);
		}
		_canInteract = false;
	}
	
	void OnEnable()
	{
		SpawnManager.SpawnLevelLoot += SpawnChest;
	}
	
	void OnDisable()
	{
		SpawnManager.SpawnLevelLoot -= SpawnChest;
	}
	
	void Awake()
	{
		//_willSpawn = false;
		
		chest = transform.GetChild(0).gameObject;
		chest.SetActive(false);
		
		chestAnimator = transform.GetChild(0).gameObject.GetComponent<Animator>();
		chestCollider = gameObject.GetComponent<BoxCollider>();
		chestCollider.enabled = false;
		
		_isOpen = false;
	}
	
	void Update()
	{
		CheckDisplayInteractionMsg();
	}
	
	void CheckDisplayInteractionMsg()
	{
		if(PlayerController.Instance.InteractionMngr._objectPresent && !_isOpen && _willSpawn)
		{
			if(PlayerController.Instance.InteractionMngr._objectInView == this.gameObject)
			{
				interactMsg.SetActive(true);
			}
			else interactMsg.SetActive(false);
		}
		else interactMsg.SetActive(false);
	}
	
	void SpawnChest()
	{
		if(!_willSpawn) return;
		chest.SetActive(true);
		chestCollider.enabled = true;
		itemInChest = SpawnManager.Instance.GenerateRandomLoot(SpawnManager.RANDOMLOOTTYPE.Any);
	}
	
	void OnMouseDown()
	{
		if(!_isOpen && _canInteract && PlayerController.Instance.PlayerState == PlayerController.PLAYERSTATE.LockedInteract) OpenChest();
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
