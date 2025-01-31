using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
	[Range(0, 1)] public float Value;
	[Space(5)]
	[SerializeField] RectTransform backgroundBar;
	[SerializeField] RectTransform progressionBar;
	[Space(5)]
	[Header("Offsets")]
	[SerializeField] float topOffset;
	[SerializeField] float bottomOffset;
	[SerializeField] float sideOffset;
	
	void Update()
	{
		UpdateProgressBar();
	}
	
	void UpdateProgressBar()
	{
		// Value
		Value = Mathf.Clamp(Value, 0, 1);
		Vector2 progress = progressionBar.sizeDelta;
		progress.y = (backgroundBar.sizeDelta.y - (topOffset + bottomOffset)) * Value;
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
