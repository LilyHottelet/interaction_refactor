
using UnityEngine;


public class NPCComponent : MonoBehaviour {

    public NPCs npcAsset;
    public Camera npcCam;
    public Transform iconSpawnPosition;
	public Phase enteringPhase;
    public Phase leavingPhase;
	public Clue requiredClue;
    private Animator animator;
    private float randomTime;   
	public Clue[] NoDialogueRemarks;
	
    private void Awake()
    {
        animator = GetComponent<Animator>();
        randomTime = Random.Range (10f, 30f);
        Invoke("ChangeAnimation", randomTime);
		
    }

    private void ChangeAnimation ()
    {
	    if ( GetComponent<Animator>() != null)
	    {
		    animator.SetTrigger ("stinger");
		    animator.SetInteger("stingerShuffle",GetRandom());
		    randomTime = Random.Range (10f, 30f);
		    Invoke("ChangeAnimation", randomTime);
	    }
        
    }

	private int GetRandom()
	{
		int randomInt = Random.Range(0, 10);
		return randomInt;
	}
	
	
}