
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseManager : ManagerEvents
{
    public static Phase currentPhase;
    public int startingPhaseIndex;
    public List<Phase> allPhases; //=> to add in editor
    public Text currentPhaseTextBox;
    
    private void Awake()
    {
        GetPhase(startingPhaseIndex);
        currentPhaseTextBox.text = currentPhase.name;
        Debug.Log(PhaseManager.currentPhase);
    }

    private void OnEnable()
    {
        ManagerEvents.FinishedPropInteraction += OnFinishedPropInteracting;
        ManagerEvents.FinishedNpcInteraction += OnFinishedNpcInteracting;
        ManagerEvents.LevelContextUpdated += OnLevelContextUpdate;
        ManagerEvents.FinishedCinematic += OnFinishCinematic;
        Invoke("TriggerBeginRemark",1f);
    }

    private void OnDisable()
    {
        ManagerEvents.FinishedPropInteraction -= OnFinishedPropInteracting;
        ManagerEvents.FinishedNpcInteraction -= OnFinishedNpcInteracting;
        ManagerEvents.LevelContextUpdated -= OnLevelContextUpdate;
        ManagerEvents.FinishedCinematic -= OnFinishCinematic;
    }

    private void OnFinishedPropInteracting(GameObject go, Props prop)
    {
        
        if (prop.clue != null)
        {            
            ProcessClue(prop.clue);
            
        }
    }

    private void OnFinishedNpcInteracting(NPCs npc,GameObject go)
    {
        foreach (var clue in npc.givenClues)
        {
            ProcessClue(clue);
        }
    }

    private void OnFinishCinematic(Clue progressClue)
    {
        SetClue(progressClue,true);
        StartCoroutine(DelayClueProcessing(progressClue));
    }

    private void ProcessClue(Clue clue)
    {
        VerifyClue(clue);
        //Check on Phase state
        if (CheckForPhaseIsComplete())
        {
            //Update if required
            UpdatePhase();
        }
    }

    private void SetClue(Clue clue,bool isFound)
    {
        clue.hasBeenFound = isFound;
    }

    private void GetPhase(int phaseIndex)
    {
        foreach (var obj in allPhases)
        {
            if (obj.phaseIndex == phaseIndex)
            {
                currentPhase = obj;
            }
        }
    }

    private void VerifyClue(Clue clue) //checks if the clue is a part of this phase and ticks it as "found"
    {
        List<Clue> currentRequiredClues = currentPhase.requiredClues;
        foreach (var obj in currentRequiredClues)
        {
            if (clue.ID == obj.ID)
            {
                obj.hasBeenFound = true;
            }
        }
    }

    private bool CheckForPhaseIsComplete() //checks if all the clues in the phase have been ticked
    {
        List<Clue> currentRequiredClues = currentPhase.requiredClues;
        foreach (var obj in currentRequiredClues)
        {
            if (!obj.hasBeenFound)
            {
                return false;
            }
        }
        return true;
    }

    private void UpdatePhase()
    {
        startingPhaseIndex++;
        GetPhase(startingPhaseIndex);
        OnDetectiveRemarkStarted(currentPhase.BeginPhaseRemark);
        OnUpdatedPhase(currentPhase);
        currentPhaseTextBox.text = currentPhase.name;
    }

    private void OnLevelContextUpdate(PlayerData data)
    {
        GetPhase(data.currentPhaseID);
        OnDetectiveRemarkStarted(currentPhase.BeginPhaseRemark);
        OnUpdatedPhase(currentPhase);
    }

    private IEnumerator DelayClueProcessing(Clue clue)
    {
        yield return new WaitForSeconds(3f);
        ProcessClue(clue);
    }

    private void TriggerBeginRemark()
    {
         OnDetectiveRemarkStarted(currentPhase.BeginPhaseRemark);   
    }
}
