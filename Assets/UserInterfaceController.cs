using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceController : MonoBehaviour
{
    public static int ActiveHotbarSlot;

    [SerializeField] private Transform hotbarCollection;

    [SerializeField] private ProgressBar healthBar;
    [SerializeField] private ProgressBar experienceBar;

    private List<HotbarSlot> hotbarSpaces = new List<HotbarSlot>();

    [HideInInspector] public InventoryManager InventoryMngr;

    private Animator guiAnimator;

    private void Start()
    {
        guiAnimator = GetComponent<Animator>();

        foreach(var slot in hotbarCollection.GetComponentsInChildren<HotbarSlot>())
        {
            hotbarSpaces.Add(slot);
        }

        ActiveHotbarSlot = 1;
    }

    public void UpdateHotbarItemImages()
    {
        HashSet<int> slotsLeft = new HashSet<int>()
        {
            0, 1, 2, 3, 4, 5, 6
        };
        foreach (var hotbarItem in InventoryMngr.HotbarItemOrder.Keys)
        {
            hotbarSpaces[InventoryMngr.HotbarItemOrder[hotbarItem]].ForegroundImage.sprite = hotbarItem.GetComponent<Item>().ItemData.Image;
            hotbarSpaces[InventoryMngr.HotbarItemOrder[hotbarItem]].ForegroundImage.gameObject.SetActive(true);
            slotsLeft.Remove(InventoryMngr.HotbarItemOrder[hotbarItem]);
        }
        foreach(var slot in slotsLeft)
        {
            hotbarSpaces[slot].ForegroundImage.gameObject.SetActive(false);
        }
    }

    public void SetHealthBar(float currentHP, float maxHP)
    {
        healthBar.UpdateProgressBar(currentHP / maxHP);
    }

    public void SetExperienceBar(float currentXP, float maxXP)
    {
        experienceBar.UpdateProgressBar(currentXP / maxXP);
    }

    public void ChangeActiveHotbar(int newTarget)
    {
        hotbarSpaces[ActiveHotbarSlot - 1].ToggleActive(false);
        hotbarSpaces[newTarget - 1].ToggleActive(true);
        ActiveHotbarSlot = newTarget;
    }

    public void Died()
    {
        guiAnimator.SetTrigger("Died");
    }

    public void Vanished()
    {
        guiAnimator.SetTrigger("Vanished");
    }
}
