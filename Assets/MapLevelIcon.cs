using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MapLevelIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
	[SerializeField] private int levelBuildIndex;
	[Space(5)]
    [SerializeField] private TextMeshProUGUI levelText;
	[SerializeField] private string levelName;
	[Space(5)]
    [SerializeField] private GameObject lockedMessage;
    public bool isLocked;

    void Start()
	{
		lockedMessage.SetActive(false);
        levelText.SetText(levelName);
    }

    public void OnPointerEnter(PointerEventData enterEventData)
	{
		if(isLocked)
			lockedMessage.SetActive(true);
	}
	
	public void OnPointerExit(PointerEventData exitEventData)
	{
        if (isLocked)
            lockedMessage.SetActive(false);
    }
	
	public void OnPointerDown(PointerEventData downEventData)
	{
		if(!isLocked)
			SceneManager.LoadScene(levelBuildIndex);
	}
}
