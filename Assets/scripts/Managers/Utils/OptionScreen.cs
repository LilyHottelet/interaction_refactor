using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionScreen : ManagerEvents
{

	public bool optionsDisplayed;
	public GameObject UIOption;
	public GameObject settingsPanel;
	public AudioClip displaySound;
	public GamePausing gamePausing;

	void Start()
	{
		gamePausing = GameObject.Find("Managers").GetComponent<GamePausing>();
		UIOption.SetActive (false);
		optionsDisplayed = false;
	}

	public void DisplayOptionScreen()
	{
		OnUIPanelDisplayed(displaySound);
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		
		UIOption.SetActive (true);
		gamePausing.PauseGame();
		optionsDisplayed = true;
	}

	public void ExitOptionScreen()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		UIOption.SetActive (false);
		gamePausing.PlayGame();
		optionsDisplayed = false;
	}

	public void QuitGame()
	{
		Application.Quit ();
	}

	public void DisplaySettings()
	{
		settingsPanel.SetActive (true);
	}

	public void RemoveSettings()
	{
		settingsPanel.SetActive (false);
	}
}
