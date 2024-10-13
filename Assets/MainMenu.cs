using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField] int startSceneBuildIndex = 0;
	
	public void StartGame()
	{
		SceneManager.LoadScene(startSceneBuildIndex);
	}
	
	public void ExitGame()
	{
		Application.Quit();
	}
}
