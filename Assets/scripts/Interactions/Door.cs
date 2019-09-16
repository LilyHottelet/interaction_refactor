using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;
[CreateAssetMenu(fileName = "New Door", menuName = "Custom/Door")]
public class Door : ScriptableObject 
{
     public bool isUnlocked;
     public string[] scenesToLoad;
     public string[] scenesToUnload;
     public AudioClip openingSound;
     public Transform spawnPoint;
     public Phase requiredPhase;
}
