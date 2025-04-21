using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Icons on the level select map that allow dungeon selection
/// </summary>

public class MapLevelIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
	[SerializeField] private int levelBuildIndex;
	[Space(5)]
    [SerializeField] private TextMeshProUGUI levelText;
	[SerializeField] private string levelName;
	[Space(5)]
    [SerializeField] private GameObject lockedMessage;
    [SerializeField] private bool isLocked;

    private void Start()
	{
		lockedMessage.SetActive(false);
        levelText.SetText(levelName);
        if (GameManager.Instance.GameData.PlayerLevel >= 3) isLocked = false;
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
