using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls treasure chest interactions
/// </summary>

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
}
