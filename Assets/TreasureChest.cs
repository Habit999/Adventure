using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
	private event Action<GameObject, int> OnGiveLoot;

	[HideInInspector] public LootSpawnManager LootMngr;

    [HideInInspector] public Item ItemInChest;

    [SerializeField] private Transform itemSpawnLocation;

    private Animation animator;

	[HideInInspector] public MimicComponent MimicHideout;

	[HideInInspector] public bool IsOpen;

	[Space(5)]

    [SerializeField] private GameObject interactMessage;

    [SerializeField] private GameObject noSpaceMessage;
	[SerializeField] private float noSpaceMessageDuration;
	private float noSpaceMessageTimer;

    private void Awake()
    {
        animator = GetComponent<Animation>();
        MimicHideout = GetComponent<MimicComponent>();

        IsOpen = false;

        noSpaceMessageTimer = 0;
    }

    private void Start()
    {
		OnGiveLoot += PlayerController.Instance.InventoryMngr.AddItem;

        interactMessage.SetActive(false);
    }

    private void Update()
    {
        if (noSpaceMessageTimer > 0) noSpaceMessageTimer -= Time.deltaTime;
		else noSpaceMessage.SetActive(false);
    }

    public void SpawnLootItem()
	{
		Item item = LootMngr.GenerateRandomLoot();
        ItemInChest = Instantiate(item, itemSpawnLocation);
		ItemInChest.transform.localPosition = Vector3.zero;
        ItemInChest.transform.localRotation = Quaternion.identity;
    }

	public void OpenChest(PlayerController player)
	{
		if (!IsOpen)
		{
			interactMessage.SetActive(false);

            if (MimicHideout != null && MimicHideout.IsMimic)
			{
				MimicHideout.TriggerMimic(player);
				IsOpen = true;
            }
			else
			{
				if(player.InventoryMngr.CheckSpaceForItem(ItemInChest))
				{
                    IsOpen = true;
                    StartCoroutine(OpenRoutine());
                    return;
                }

                noSpaceMessage.SetActive(true);
                noSpaceMessageTimer = noSpaceMessageDuration;
            }
        }
    }

	public void HitChest()
	{
		if (MimicHideout != null && MimicHideout.IsMimic)
		{
			MimicHideout.DeactivateComponent();
		}
	}

    private IEnumerator OpenRoutine()
	{
		animator.Play();
		yield return new WaitForSeconds(animator.clip.length);
        itemSpawnLocation.gameObject.SetActive(false);
		OnGiveLoot?.Invoke(ItemInChest.gameObject, 1);
    }

    private void OnTriggerEnter(Collider enterTrigger)
    {
        if(enterTrigger.gameObject.tag == "Player" && !IsOpen)
			interactMessage.SetActive(true);
    }

    private void OnTriggerExit(Collider exitTrigger)
    {
        if (exitTrigger.gameObject.tag == "Player")
            interactMessage.SetActive(false);
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
