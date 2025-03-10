using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private TMP_Dropdown screenResolution;

	public void StartGame()
	{
		SceneManager.LoadScene(1);
	}
	
	public void ExitGame()
	{
		Application.Quit();
	}

	public void ChangeResolution()
	{
		string selectedOption = screenResolution.captionText.ToString();
		string[] resolution = selectedOption.Split('x', ' ');
		Screen.SetResolution(int.Parse(resolution[0]), int.Parse(resolution[1]), true);
	}
}
