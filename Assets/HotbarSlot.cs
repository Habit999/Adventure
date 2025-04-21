using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Contains hotbar functionality
/// </summary>

public class HotbarSlot : MonoBehaviour
{
    [SerializeField] private Color activeHotbarColor;
    [SerializeField] private Color inactiveHotbarColor;

    [Space(5)]

    [SerializeField] private Image backgroundImage;
    public Image ForegroundImage;

    private void Start()
    {
        ForegroundImage.gameObject.SetActive(false);
    }

    public void ToggleActive(bool isActive)
    {
        backgroundImage.color = isActive ? activeHotbarColor : inactiveHotbarColor;
    }
}
