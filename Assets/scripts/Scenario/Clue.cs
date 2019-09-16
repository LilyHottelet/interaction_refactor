using UnityEngine;

[CreateAssetMenu(fileName = "New Clue",menuName = "Custom/Clue")]
public class Clue : ScriptableObject
{
    [Header("General")]
    public string ID;
    public bool hasBeenFound;
    public TypeOfClue type;
    public bool isJournalEntry;
    
    
    [Header("For MCCOY Remarks")]
    public AudioClip detectiveRemarkAudio;
    public string detectiveRemarkText;
    
    [Header("For JournalDisplay")]
    public Sprite noteBookSprite;
    public string title;
    public string flavourText;
    
    public enum TypeOfClue
    {
        Evidence,
        Portait
    }
}



