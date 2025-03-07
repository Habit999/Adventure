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
    }

    public void UpdateHotbarItemImages()
    {
        for (int i = 0; i < hotbarSpaces.Count; i++)
        {
            foreach (var hotbarItem in InventoryMngr.HotbarItemOrder.Keys)
            {
                if (InventoryMngr.HotbarItemOrder[hotbarItem] == i)
                {
                         hotbarSpaces[i].ForegroundImage.sprite = hotbarItem.GetComponent<Item>().ItemData.Image;
                }
            }
        }
    }

    public void SetHealthBar(float currentHP, float maxHP)
    {
        healthBar.UpdateProgressBar(currentHP / maxHP);
    }

    public void SetExperienceBar(float currentXP, float maxXP)
    {
        healthBar.UpdateProgressBar(currentXP / maxXP);
    }

    public void ChangeActiveHotbar(int newTarget)
    {
        hotbarSpaces[ActiveHotbarSlot].ToggleActive(false);
        hotbarSpaces[newTarget - 1].ToggleActive(true);
        ActiveHotbarSlot = newTarget - 1;
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
