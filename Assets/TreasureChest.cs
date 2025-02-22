using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
	[HideInInspector] public Item ItemInChest;

    private Transform itemSpawnLocation;

    private Animation animator;

	private bool isOpen;

	private void Awake()
    {
        animator = GetComponent<Animation>();
    }

    public void SpawnLootItem(Item lootItem)
	{
		print(lootItem);
        ItemInChest = Instantiate(lootItem, itemSpawnLocation);
    }

	public void OpenChest()
	{
		if (!isOpen)
		{
            isOpen = true;
			StartCoroutine(OpenRoutine());
        }
    }

	private IEnumerator OpenRoutine()
	{
		animator.Play();
		yield return new WaitForSeconds(animator.clip.length);
        itemSpawnLocation.gameObject.SetActive(false);
    }

    /*public bool WillSpawn;
	
	[Space(5)]
	
	[SerializeField] GameObject interactMsg;
	
	[HideInInspector] public bool IsOpen;
	
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
			IsOpen = true;
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
		chest = transform.GetChild(0).gameObject;
		chest.SetActive(false);
		
		chestAnimator = transform.GetChild(0).gameObject.GetComponent<Animator>();
		chestCollider = gameObject.GetComponent<BoxCollider>();
		chestCollider.enabled = false;
		
		IsOpen = false;
	}
	
	void Update()
	{
		CheckDisplayInteractionMsg();
	}
	
	void CheckDisplayInteractionMsg()
	{
		if(PlayerController.Instance.InteractionMngr.ObjectPresent && !IsOpen && WillSpawn)
		{
			if(PlayerController.Instance.InteractionMngr.ObjectInView == this.gameObject)
			{
				interactMsg.SetActive(true);
			}
			else interactMsg.SetActive(false);
		}
		else interactMsg.SetActive(false);
	}
	
	void SpawnChest()
	{
		if(!WillSpawn) return;
		chest.SetActive(true);
		chestCollider.enabled = true;
		itemInChest = SpawnManager.Instance.GenerateRandomLoot(SpawnManager.RANDOMLOOTTYPE.Any);
	}
	
	#if UNITY_EDITOR
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Transform chestModel = transform.GetChild(0);
		Gizmos.DrawLine(chestModel.position, chestModel.position + -chestModel.right);
	}
	
	#endif*/
}
