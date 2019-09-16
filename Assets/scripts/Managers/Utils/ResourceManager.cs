using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
	private void Awake()
	{
		foreach (var obj in Resources.LoadAll("Clues",typeof(Clue)))
		{
			Clue temp = obj as Clue;
			temp.hasBeenFound = false;
		}

		foreach (var prop in Resources.LoadAll("Props",typeof(Props)))
		{
			Props temp = prop as Props;
			temp.animationTriggered = false;
		}
		
		foreach (var npc in Resources.LoadAll("NPC",typeof(NPCs)))
		{
			NPCs temp = npc as NPCs;
			temp.hasBeenMet = false;
			temp.savedDialogueState = null;
			temp.givenClues.Clear();
		}
		
	}
}
