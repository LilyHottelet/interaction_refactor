using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class SpecialEvents : MonoBehaviour
{

	public Phase phoneEventPhase;
	public Phase greetHaroldPhase;
	public AudioClip ringClip;
	private bool ring;
	public AudioSource soundSource;


	private void OnEnable()
	{
		ManagerEvents.UpdatedPhase += OnPhaseUpdate;
		ManagerEvents.StartedNpcInteraction += OnStartedNpcInteracting;
	}

	private void OnDisable()
	{
		ManagerEvents.UpdatedPhase -= OnPhaseUpdate;
		ManagerEvents.StartedNpcInteraction -= OnStartedNpcInteracting;
	}

	private void OnPhaseUpdate(Phase phase)
	{
		if (phase==phoneEventPhase)
		{
			ring = true;
		}

		
	}

	private void OnStartedNpcInteracting(NPCs npc, GameObject go, string passage)
	{
		if (npc.name == "Phone")
		{
			soundSource.Stop();
		}
	}

	private void Update()
	{
		if (ring && GameManager.currentScene == "Clarisse_Lobby")
		{
			soundSource.Play();
			ring = false;
		}

//		if (Input.GetKeyDown(KeyCode.M))
//		{
//			soundSource.Stop();
//		}
	}
}
