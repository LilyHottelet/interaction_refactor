using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fading : MonoBehaviour {

	public Animator fadeAnim;
	public AnimationClip fadeIn;
	public AnimationClip fadeOut;
	//[HideInInspector]
	public float totalFadeDuration;
	private bool alreadyFadedIn = false;

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	public void StartFadeOut()
	{
		alreadyFadedIn = false;
		fadeAnim.SetTrigger("startFadeOut");
	}
	public void StartFadeIn()
	{
		alreadyFadedIn = true;
		StartCoroutine(StartFadingIn());
		
	}

	public void StartFadeInOut()
	{
		fadeAnim.SetTrigger("startFadeOut");
		//yield return new WaitForSeconds(fadeOut.length);
		fadeAnim.SetTrigger("fadeIn");
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "game_kit" || alreadyFadedIn)
		{
			return;
		}
		Debug.Log("I'm supposed to fade in here");
		StartFadeIn();
	}

	private IEnumerator StartFadingIn()
	{
		yield return new WaitForSeconds(0.1f);
		fadeAnim.SetTrigger("startFadeIn");
		Debug.Log("test fade");
	}

}
