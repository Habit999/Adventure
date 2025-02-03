using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceController : MonoBehaviour
{
    public static int ActiveHotbarSlot;

    [SerializeField] private Transform hotbarCollection;

    private List<HotbarSlot> hotbarSpaces = new List<HotbarSlot>();

    private void Start()
    {
        foreach(Transform slot in hotbarCollection)
        {
            hotbarSpaces.Add(slot.gameObject.GetComponent<HotbarSlot>());
        }
    }

    public void ChangeActiveHotbar(int newTarget)
    {
        hotbarSpaces[ActiveHotbarSlot].ToggleActive(false);
        hotbarSpaces[newTarget].ToggleActive(true);
        ActiveHotbarSlot = newTarget;
    }
}
