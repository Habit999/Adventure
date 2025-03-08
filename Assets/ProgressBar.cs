using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
