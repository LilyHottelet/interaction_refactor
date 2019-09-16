using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
[System.Serializable]
[CreateAssetMenu(fileName = "New Prop", menuName = "Custom/Prop")]
public class Props : ScriptableObject
{
	[Header("General")]
    public TypeOfProp type;
	public bool hasNextAction;
	public bool canInteractClose;
	public bool isNestedAction;
	public bool hasAnimation;
	public bool animationTriggered;
	public bool isDrawer;
	
	[Space(10)]
	
	public bool isCollectable;

	[Space(10)] 
	public Sprite readingMaterial;

	[Space(10)] 
	public Door door;
	
	[Header("Assigned Clue")]
    public Clue clue;

	

    public enum TypeOfProp
    {
        Inspect,
        Sound,
        Reaction,
        Look,
        Read,
        Door,
	    Elevator,
	    Kinetoscope
    }
		
}