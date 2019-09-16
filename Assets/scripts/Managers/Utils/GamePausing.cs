using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using I18N.Common;
using UnityEngine;

public class GamePausing : ManagerEvents {

    public GameObject Player;
    private PlayerController playerController;
    private footSteps footStepsScript;
    private InteractionDetector interactionDetector;
    private Animator animator;
    private Phase[] noPlayerPhases;

    private void Awake()
    {
        noPlayerPhases = GameObject.Find("Managers").GetComponent<GameManager>().noPlayerPhases;
        playerController = Player.GetComponent<PlayerController>();
        footStepsScript = Player.GetComponent<footSteps>();
        interactionDetector = GameObject.Find("Input").GetComponent<InteractionDetector>();
        animator = Player.GetComponent<Animator>();
        
    }

    private void OnEnable()
    {
        ManagerEvents.StartedPropInteraction += OnStartedPropInteracting;
        ManagerEvents.StartedNpcInteraction += OnStartedNpcInteracting;
        ManagerEvents.FinishedPropInteraction += OnFinishedPropInteracting;
        ManagerEvents.FinishedNpcInteraction += OnFinishedNpcInteracting;
        ManagerEvents.LoadedEnvironment += OnEnvironmentLoaded;
    }

    private void OnDisable()
    {
        ManagerEvents.LoadedEnvironment -= OnEnvironmentLoaded;
        ManagerEvents.StartedPropInteraction -= OnStartedPropInteracting;
        ManagerEvents.StartedNpcInteraction -= OnStartedNpcInteracting;
        ManagerEvents.FinishedPropInteraction -= OnFinishedPropInteracting;
        ManagerEvents.FinishedNpcInteraction -= OnFinishedNpcInteracting;
    }

    private void OnStartedPropInteracting(GameObject go,Props prop)
    {
        switch (prop.type)
        {
            case Props.TypeOfProp.Look:
            case Props.TypeOfProp.Read:
            case Props.TypeOfProp.Inspect:
            case Props.TypeOfProp.Kinetoscope:
                PauseGame();
                break;
        }
    }

    private void OnStartedNpcInteracting(NPCs npc,GameObject go, string passage)
    {
        PauseGame();
        Debug.Log("testPause");

    }

    private void OnFinishedPropInteracting(GameObject GO,Props prop)
    {
        switch (prop.type)
        {
            case Props.TypeOfProp.Read:
            case Props.TypeOfProp.Inspect:
                if (!prop.isNestedAction)
                {
                    PlayGame();
                }
                else
                {
                    EnableCloseInteraction();  
                }
                break;
            case Props.TypeOfProp.Kinetoscope:
                PlayGame();
                break;
        }

    }

    private void OnFinishedNpcInteracting(NPCs npc,GameObject go)
    {
        if (!CheckForNoPlay())
        {
            PlayGame();
            Debug.Log("testplay"); 
        }
    }

    private void OnEnvironmentLoaded()
    {
        playerController.ResetCamRotation();
        playerController.enabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(DelayPlayerMovement());
    }
	
    public void PauseGame ()
    {

        playerController.enabled = false;
        footStepsScript.enabled = false;
        interactionDetector.enabled = false;
        animator.enabled = false;

    }

    public void PlayGame()
    {
        playerController.isAbleToWalk = true;
        playerController.enabled = true;
        footStepsScript.enabled = true;
        interactionDetector.enabled = true;
        animator.enabled = true;
    }

    public void BlockCamera ()
    {
        playerController.enabled = false;
        animator.enabled = true;
    }

    public void EnableCloseInteraction()
    {
        interactionDetector.enabled = true;
    }
    
    private IEnumerator DelayPlayerMovement()
    {
        yield return new WaitForSeconds(0.7f);
        PlayGame();
    }
    
    private bool CheckForNoPlay()
    { 
        foreach (var noPlayPhase in noPlayerPhases)
        {
            if (PhaseManager.currentPhase == noPlayPhase)
            {
                return true;
            }
        }

        return false;

    }
}