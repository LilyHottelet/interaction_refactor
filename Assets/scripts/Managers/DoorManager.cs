using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorManager : ManagerEvents
{

	public List<Door> allDoors;
	
	private void Awake()
	{
		CreateDoorList();
		SetLocks();
	}

	private void OnEnable()
	{
		ManagerEvents.StartedPropInteraction += OnStartedPropInteracting;
		ManagerEvents.UpdatedPhase += OnPhaseUpdate;
	}

	private void OnDisable()
	{
		ManagerEvents.StartedPropInteraction -= OnStartedPropInteracting;
		ManagerEvents.UpdatedPhase -= OnPhaseUpdate;
	}

	private void CreateDoorList()
	{
		foreach (var obj in Resources.LoadAll("Doors",typeof(Door)))
		{
			Door door = obj as Door;
			allDoors.Add(door);
		}
	}

	private void OnStartedPropInteracting(GameObject go, Props prop)
	{
		if (prop.type == Props.TypeOfProp.Door)
		{
			
			Door targetDoor = prop.door;
			if (targetDoor.isUnlocked)
			{
				OnDoorOpened(targetDoor);
			}
			else
			{
				OnDetectiveRemarkStarted(prop.clue);
			}

			StartCoroutine(EndInteraction(go, prop));

		}
	}

	private void OnPhaseUpdate(Phase phase)
	{
		Debug.Log("test event");
		foreach (var door in allDoors)
		{
			if (door.requiredPhase == phase)
			{
				door.isUnlocked = true;
			}
		}
	}

	private void SetLocks()
	{
		foreach (var door in allDoors)
		{
			door.isUnlocked = false;
		}
	}

	private IEnumerator EndInteraction(GameObject go, Props prop)
	{
		yield return new WaitForSeconds(1f);
		OnFinishedPropInteraction(go,prop);
	}
}
