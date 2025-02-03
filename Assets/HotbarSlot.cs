using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    [SerializeField] private Color activeHotbarColor;
    [SerializeField] private Color inactiveHotbarColor;

    [Space(5)]

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image foregroundImage;

    [HideInInspector] public Sprite ItemImage;

    public void ToggleActive(bool isActive)
    {
        backgroundImage.color = isActive ? activeHotbarColor : inactiveHotbarColor;
        foregroundImage.sprite = isActive ? ItemImage : null;
    }
}
