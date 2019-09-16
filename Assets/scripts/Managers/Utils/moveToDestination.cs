using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class moveToDestination : MonoBehaviour {

	public Transform goal;
	public NavMeshAgent agent;

	void Start () 
	{
		agent = GetComponent<NavMeshAgent> ();
		agent.destination = goal.position;
	}
	
	void Update () {
		
	}
}
