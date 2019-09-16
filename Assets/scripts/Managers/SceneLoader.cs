using System.Collections;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : ManagerEvents
{
	private Fading fading;
	
	private void Awake()
	{
		fading = GameObject.Find("fade").GetComponent<Fading>();
		LoadScene(GameManager.currentScene);
		OnLoadedEnvironment();
	}

	private void OnEnable()
	{
		ManagerEvents.DoorOpened += OnDoorOpen;
		ManagerEvents.ChangedFloor += OnFloorChange;
		ManagerEvents.MustLoadScene += OnMustLoad;
	}

	private void OnDisable()
	{
		ManagerEvents.DoorOpened -= OnDoorOpen;
		ManagerEvents.ChangedFloor -= OnFloorChange;
		ManagerEvents.MustLoadScene -= OnMustLoad;
	}

	private void OnFloorChange(ElevatorButton button)
	{
		LoadScene(button.sceneToLoad);
		string [] currentFloor = new string[1];
		currentFloor[0]=GameManager.currentScene;
		StartCoroutine(UnloadScene(currentFloor));

	}

	private void OnDoorOpen(Door door)
	{
		StartCoroutine(FadingDoor(door));
	}
	
	private IEnumerator FadingDoor(Door door)
	{
		fading.StartFadeOut();
		yield return new WaitForSeconds(1f);
		foreach (var room in door.scenesToLoad)
		{
			LoadScene(room);
		}
		StartCoroutine(UnloadScene(door.scenesToUnload));
		OnChangedRoom(door);
		OnLoadedEnvironment();
		Debug.Log("Scene is loaded");
	}
	
	private void OnMustLoad(Room roomToLoad)
	{
		StartCoroutine(RegularFadeOut(roomToLoad.sceneName));
	}

	private void LoadScene(string scene)
	{
		SceneManager.LoadScene(scene, LoadSceneMode.Additive);
		if (scene == "Clarisse_ascenseur_baked" || scene == "game_kit")
		{
			return;
		}
		
		StartCoroutine(SetCurrentScene(scene));
		

	}

	private IEnumerator UnloadScene(string[] scenes)
	{
		yield return new WaitForSeconds(0.5f);
		foreach (var scene in scenes)
		{
			SceneManager.UnloadSceneAsync(scene);
		}
		
	}

	private IEnumerator SetCurrentScene(string currentscene)
	{
		yield return new WaitForSeconds(1f);
		GameManager.currentScene = currentscene;
		Debug.Log("current scene " + GameManager.currentScene);
	}

	private IEnumerator RegularFadeOut(string sceneToLoad)
	{
		fading.StartFadeOut();
		yield return new WaitForSeconds(1f);
		SceneManager.UnloadSceneAsync(GameManager.currentScene);
		LoadScene(sceneToLoad);
	}

	
}
