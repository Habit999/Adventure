using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MapLevelIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
	[HideInInspector] public string _progressionStatus;
	
	public SO_LevelDetails _levelDetails;
	[Space(5)]
	[SerializeField] GameObject description;
	
	void Start()
	{
		description.SetActive(false);
	}

	public void OnPointerEnter(PointerEventData enterEventData)
	{
		description.GetComponent<TextMeshProUGUI>().SetText($"{_levelDetails.LevelName}\n\n\"{_levelDetails.LevelBrief}\"\n\n{_progressionStatus}");
		description.SetActive(true);
	}
	
	public void OnPointerExit(PointerEventData exitEventData)
	{
		description.SetActive(false);
	}
	
	public void OnPointerDown(PointerEventData downEventData)
	{
		SceneManager.LoadScene(_levelDetails.LevelBuildIndex);
	}
}
