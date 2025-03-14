using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private Button continueButton;
	[SerializeField] private GameObject areYouSurePanel;

    private void Start()
    {
        if (GameManager.Instance.GameData == null) continueButton.interactable = false;
		else continueButton.interactable = true;

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
