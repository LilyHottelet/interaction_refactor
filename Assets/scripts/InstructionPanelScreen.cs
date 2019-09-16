using System.Collections;
using UnityEngine;

public class InstructionPanelScreen : ManagerEvents
{

	public Animator panelAnimator;
	public AnimationClip panelAnimationClip;
	
	public GameObject instructionPanel;
	public AudioClip displaySound;
	
	private void Awake()
	{
		instructionPanel.SetActive(false);
	}

	public void EnablePanel()
	{
		OnUIPanelDisplayed(displaySound);
		instructionPanel.SetActive(true);
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
	public void OnClickGotit()
	{
		StartCoroutine(GotIt());
	}

	private IEnumerator GotIt()
	{
		panelAnimator.SetTrigger("got_it");
		yield return new WaitForSeconds(panelAnimationClip.length);
		OnInstructionPanelEnded();
	}
	
	public delegate void InstructionPanelEndedEventHandler();
	public static event InstructionPanelEndedEventHandler InstructionPanelEnded;

	protected virtual void OnInstructionPanelEnded()
	{
		if (InstructionPanelEnded!=null)
		{
			InstructionPanelEnded();
		}
	}
	
	
}
