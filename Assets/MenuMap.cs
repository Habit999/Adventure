using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuMap : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[Tooltip("Position the map moves to when hovered over my the players mouse")]
	[SerializeField] float hoverPositionOffset;
	[SerializeField] float transitionSpeed;
	
	RectTransform rectTransform;
	
	Vector2 startPosition;
	
	bool hovered;
	
	void Start()
	{
		rectTransform = gameObject.GetComponent<RectTransform>();
		startPosition = rectTransform.anchoredPosition;
		
		transform.GetChild(0).gameObject.SetActive(false);
		
		hovered = false;
	}
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		hovered = true;
	}
	
	public void OnPointerExit(PointerEventData eventData)
	{
		hovered = false;
	}
	
	void Update()
	{
		if(hovered)
		{
			if(rectTransform.anchoredPosition.y < hoverPositionOffset)
			{
				rectTransform.anchoredPosition += new Vector2(0, 1) * transitionSpeed * Time.deltaTime;
				transform.GetChild(0).gameObject.SetActive(true);
			}
		}
		else
		{
			if(rectTransform.anchoredPosition.y > startPosition.y)
			{
				rectTransform.anchoredPosition -= new Vector2(0, 1) * transitionSpeed * Time.deltaTime;
				transform.GetChild(0).gameObject.SetActive(false);
			}
		}
	}
}
