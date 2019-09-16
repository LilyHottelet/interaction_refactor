using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footSteps : MonoBehaviour {

	private PlayerController playerCC;
	public AudioSource source;
	public AudioClip  footStep,longStep;
	public float lowestPitch,highestPitch;
	public float lowestVolume,highestVolume;


	void Start()
	{
		playerCC = gameObject.GetComponent<PlayerController> ();

	}

	void Update()
	{
		if (playerCC.isRunning && !source.isPlaying)
		{
			source.pitch = Random.Range (lowestPitch,highestPitch);
			source.volume = Random.Range (lowestVolume,highestVolume);
			source.PlayOneShot (footStep);

		}
		if (playerCC.isWalking && !source.isPlaying)
		{
			source.pitch = Random.Range (lowestPitch,highestPitch);
			source.volume = Random.Range (lowestVolume,highestVolume);
			source.PlayOneShot (longStep);

		}
	}
}

