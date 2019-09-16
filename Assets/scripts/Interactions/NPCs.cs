using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "New NPC", menuName = "Custom/NPC")]
public class NPCs : ScriptableObject
{
	public Clue portrait;//-> contains info of portrait
	public bool hasBeenMet;
	public TextAsset dialogue;
	public string savedDialogueState;
	public List<Clue> givenClues = new List<Clue>();

}
