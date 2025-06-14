using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controls the games main menu
/// </summary>

public class MainMenu : MonoBehaviour
{
	[SerializeField] private Button continueButton;
	[SerializeField] private GameObject areYouSurePanel;

    private void Start()
    {
        if (GameManager.Instance.LoadGame()) continueButton.interactable = true;
		else continueButton.interactable = false;

		areYouSurePanel.SetActive(false);
    }

    public void StartGame()
	{
		GameManager.Instance.CurrentGameState = GameManager.GameStates.InGame;
        SceneManager.LoadScene(1);
	}

	public void NewGame()
	{
		if(GameManager.Instance.GameData != null)
		{
			areYouSurePanel.SetActive(true);
        }
		else
		{
			StartGame();
        }
    }

	public void NewGameWipe()
	{
		GameManager.Instance.ClearGameData();
        StartGame();
    }

    public void ExitGame()
	{
		Application.Quit();
	}
}
