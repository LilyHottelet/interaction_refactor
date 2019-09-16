using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PostProcessSwapper : MonoBehaviour
{
	public GameObject playerCam;

	public void Swap(PostProcessingProfile newProfile)
	{
		playerCam.GetComponent<PostProcessingBehaviour>().profile = newProfile;
	}
}
