using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI fullscreenButton;

	public void StartGame()
	{
		SceneManager.LoadScene(1);
	}
	
	public void ExitGame()
	{
		Application.Quit();
	}

	public void ChangeFullscreenOption()
	{
		GameManager.Instance.ChangeWindowMode();
		fullscreenButton.SetText(GameManager.Instance.IsFullscreen ? "X" : "");
	}
}
