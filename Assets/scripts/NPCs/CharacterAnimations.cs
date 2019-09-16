using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour {

	public Animator gloriaAnim,haroldAnim;
	private float randomTime;

	void Start()
	{
		randomTime = Random.Range (10f, 80f);
		InvokeRepeating ("ChangeAnimation", 2f, randomTime);
	}

	public void ChangeAnimation ()
	{
		gloriaAnim.SetTrigger ("stinger");
		haroldAnim.SetTrigger ("stinger");
	}
}