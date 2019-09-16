using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionDisplay : MonoBehaviour {
	
	public Text bottomText;

	
	public void Intro()
	{
		bottomText.text = "Pour vous déplacer, utilisez les touches Z,Q,S,D du clavier.";
		StartCoroutine (NextText ());	
	}
	
	IEnumerator NextText()
	{
		yield return new WaitForSeconds (3f);
		bottomText.text = "Pour courir, restez appuyé sur la touche MAJ";
		StartCoroutine (EraseText ());

	}

	IEnumerator EraseText()
	{
		yield return new WaitForSeconds (2f);
		bottomText.text = "";
	}
	
	//@TODO replace bottomtext display with actual instruction window
}
