using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameQuit : MonoBehaviour {

	private void Start()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	public void QuitGame()
	{
		Debug.Log("quitting");
		Application.Quit();
	}
}
