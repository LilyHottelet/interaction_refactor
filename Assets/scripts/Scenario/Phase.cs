using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Phase", menuName = "Custom/Phase")]
[System.Serializable]
public class Phase : ScriptableObject
{
	public int phaseIndex;
	public List<Clue> requiredClues = new List<Clue>();
	public Clue BeginPhaseRemark;

}
