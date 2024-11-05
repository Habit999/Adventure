using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MapLevelIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
	[SerializeField] SO_LevelDetails levelDetails;
	[Space(5)]
	[SerializeField] GameObject description;
	
	void Start()
	{
		// Getting child object if it cant find description
		if(description == null && transform.childCount > 0)
			description = transform.GetChild(0).gameObject;
			
		if(description != null)
		{
			description.SetActive(false);
			description.GetComponent<TextMeshProUGUI>().SetText($"{levelDetails.LevelName}\n\n{levelDetails.LevelBrief}\n\nIncomplete");
		}
	}

	public void OnPointerEnter(PointerEventData enterEventData)
	{
		description.SetActive(true);
	}
	
	public void OnPointerExit(PointerEventData exitEventData)
	{
		description.SetActive(false);
	}
	
	public void OnPointerDown(PointerEventData downEventData)
	{
		SceneManager.LoadScene(levelDetails.LevelBuildIndex);
	}
}
