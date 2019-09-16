using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Elevator : ManagerEvents
{


	public GameObject frontDoors;
	public GameObject backDoors;
	public GameObject groundFloorButton;
	public GameObject firstFloorButton;
	public Animator frontDoorsAnimator;
	public Animator backDoorsAnimator;
	public Animator effectAnimator;
	public bool isClosed = true;
	//public Scene elevator_scene;

	private void OnEnable()
	{
		ManagerEvents.CalledElevator += OnElevatorCall;
		ManagerEvents.ChangedFloor += OnFloorChange;
		Invoke("SetColliders",2f);
	}

	private void OnDisable()
	{
		ManagerEvents.CalledElevator -= OnElevatorCall;
		ManagerEvents.ChangedFloor -= OnFloorChange;

	}
	private void OnFloorChange(ElevatorButton button)
	{
		DisableColliders();
		if (GameManager.currentScene == "Clarisse_Lobby" && !isClosed)
		{
			PlayDoorAnimation(false, frontDoorsAnimator);
		}
		else
		{
			if (!isClosed)
			{
				PlayDoorAnimation(false,backDoorsAnimator);
			}
		}
		EffectAnimation(true);
		StartCoroutine(OpeningDoorsAutomatic(button.sceneToLoad));
	}

	private void OnElevatorCall()
	{
		if (isClosed)
		{
			if (GameManager.currentScene == "Clarisse_Lobby")
			{
				PlayDoorAnimation(true,frontDoorsAnimator);
				StartCoroutine(ClosingDoorsAutomatic(frontDoorsAnimator));
			}
			else
			{
				PlayDoorAnimation(true,backDoorsAnimator);
				StartCoroutine(ClosingDoorsAutomatic(backDoorsAnimator));
			}
		}
		
	}
	
	private void PlayDoorAnimation(bool isOpening,Animator doorAnimator)
	{
		doorAnimator.SetTrigger(isOpening ? "open_doors" : "close_doors");
		//isClosed = !isOpening;
	}

	private IEnumerator ClosingDoorsAutomatic(Animator doorAnimator)
	{
		yield return new WaitForSeconds(10f);
		if (!isClosed)
		{
			PlayDoorAnimation(false,doorAnimator);
		}
	}
	
	private IEnumerator OpeningDoorsAutomatic(string floorName)
	{
		yield return new WaitForSeconds(9f);
		SetColliders();
		if (floorName == "Clarisse_Lobby")
		{
			PlayDoorAnimation(true,frontDoorsAnimator);
			StartCoroutine(ClosingDoorsAutomatic(frontDoorsAnimator));
		}
		else
		{
			PlayDoorAnimation(true,backDoorsAnimator);
			StartCoroutine(ClosingDoorsAutomatic(backDoorsAnimator));
		}
		EffectAnimation(false);
		
	}

	private void SetColliders()
	{
		Debug.Log(GameManager.currentScene);
		if (GameManager.currentScene == "Clarisse_Lobby")
		{
			frontDoors.GetComponent<BoxCollider>().enabled = true;
			backDoors.GetComponent<BoxCollider>().enabled = false;
			groundFloorButton.GetComponent<BoxCollider>().enabled = false;
			firstFloorButton.GetComponent<BoxCollider>().enabled = true;
		}
		else
		{
			frontDoors.GetComponent<BoxCollider>().enabled = false;
			backDoors.GetComponent<BoxCollider>().enabled = true;
			groundFloorButton.GetComponent<BoxCollider>().enabled = true;
			firstFloorButton.GetComponent<BoxCollider>().enabled = false;
		}
	}

	private void DisableColliders()
	{
		frontDoors.GetComponent<BoxCollider>().enabled = false;
		backDoors.GetComponent<BoxCollider>().enabled = false;
		groundFloorButton.GetComponent<BoxCollider>().enabled = false;
		firstFloorButton.GetComponent<BoxCollider>().enabled = false;
	}

	private void EffectAnimation(bool isStarting)
	{
		effectAnimator.SetTrigger(isStarting ? "start_shake" : "stop_shake");
	}

}
