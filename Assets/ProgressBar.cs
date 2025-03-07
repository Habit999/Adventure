using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
	[SerializeField] private RectTransform backgroundBar;
	[SerializeField] private RectTransform progressionBar;
	[Space(5)]
	[Header("Offsets")]
	[SerializeField] private float topOffset;
	[SerializeField] private float bottomOffset;
	[SerializeField] private float sideOffset;

    public void UpdateProgressBar(float value)
	{
		// Value
		value = Mathf.Clamp(value, 0, 1);
		Vector2 progress = progressionBar.sizeDelta;
		progress.y = (backgroundBar.sizeDelta.y - (topOffset + bottomOffset)) * value;
		progress.x = backgroundBar.sizeDelta.x - sideOffset;
		progressionBar.sizeDelta = progress;
		
		// Check Offset
		Vector2 barPosition = progressionBar.anchoredPosition;
		if(barPosition.y != bottomOffset) 
		{
			barPosition.y = bottomOffset;
			progressionBar.anchoredPosition = barPosition;
		}
	}
}
