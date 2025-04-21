using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls fill percentage of UI bars e.g. health bar
/// </summary>

public class ProgressBar : MonoBehaviour
{
	[SerializeField] private Image progressionBar;

    public void UpdateProgressBar(float value)
	{
		// Value
		value = Mathf.Clamp(value, 0, 1);
		progressionBar.fillAmount = value;
    }
}
