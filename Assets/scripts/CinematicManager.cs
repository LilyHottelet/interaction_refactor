using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class CinematicManager : ManagerEvents
{

	public Phase haroldCinematicPhase;
	public Phase paxtonCinematicPhase;
	public Phase endDemoPhase;

	public Room haroldCinematicScene;
	public Room paxtonCinematicScene;
	public Room endDemoScene;
	
	private void OnEnable()
	{
		ManagerEvents.DetectiveRemarkEnded += OnDetectiveRemarkEnd;
	}

	private void OnDisable()
	{
		ManagerEvents.DetectiveRemarkEnded -= OnDetectiveRemarkEnd;
	}

	private void OnDetectiveRemarkEnd(Clue clue)
	{
		if (PhaseManager.currentPhase == haroldCinematicPhase)
		{
			StartCoroutine(LaunchCinematic(haroldCinematicScene,true));
			
		}
		else if (PhaseManager.currentPhase == paxtonCinematicPhase)
		{
			StartCoroutine(LaunchCinematic(paxtonCinematicScene,false));
		}
		else if (PhaseManager.currentPhase == endDemoPhase)
		{
			StartCoroutine(LaunchCinematic(endDemoScene,false));
		}
		
	}

	private IEnumerator LaunchCinematic(Room roomToLoad, bool elevator)
	{
		yield return new WaitForSeconds(1f);
		if (elevator)
		{
			SceneManager.UnloadSceneAsync("Clarisse_ascenseur_baked");
		}
		OnMustLoadScene(roomToLoad);
	}
}
