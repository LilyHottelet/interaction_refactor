using System.Collections;
using UnityEngine;

public class GameManager : ManagerEvents
{

    [Header("Script References")]
    public OptionScreen optionScreen;
    public JournalScreen journalScreen;
    public GamePausing gamePausing;
    public PlayerController playerController;
    
    public static string currentScene;
    public string startingScene;

    public Phase[] noPlayerPhases;
    
    private InstructionPanelScreen instructionPanelScreen;
    public static bool isBusy = false;
    public bool isInteracting = false;
    private PlayerData currentPlayerContext; 

    private void Awake()
    {
        currentScene = startingScene;
    }

    private void OnEnable()
    {
        InstructionPanelScreen.InstructionPanelEnded += OnInstructionPanelEnd;
        ManagerEvents.DetectiveRemarkStarted += OnDetectiveRemarkStart;
        ManagerEvents.DetectiveRemarkEnded += OnDetectiveRemarkEnd;
        ManagerEvents.StartedPropInteraction += OnStartedPropInteracting;
        ManagerEvents.FinishedPropInteraction += OnFinishedPropInteracting;
        ManagerEvents.UpdatedPhase += OnPhaseUpdate;
    }

    private void OnDisable()
    {
        InstructionPanelScreen.InstructionPanelEnded -= OnInstructionPanelEnd;
        ManagerEvents.DetectiveRemarkStarted -= OnDetectiveRemarkStart;
        ManagerEvents.DetectiveRemarkEnded -= OnDetectiveRemarkEnd;
        ManagerEvents.StartedPropInteraction -= OnStartedPropInteracting;
        ManagerEvents.FinishedPropInteraction -= OnFinishedPropInteracting;
        ManagerEvents.UpdatedPhase -= OnPhaseUpdate;

    }

    private void StartGame()
    {
        gamePausing.PauseGame();
        Invoke("TriggerPanel",0.5f);
    }
    
    private void Update()
    {
        //OptionScreen
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!optionScreen.optionsDisplayed)
            {
                optionScreen.DisplayOptionScreen();
            }
            else
            {
                optionScreen.ExitOptionScreen();
            }
        }

        //JournalScreen
        if (Input.GetKeyDown(KeyCode.J) && !isInteracting)
        {
            Debug.Log("journal");
            if (!journalScreen.journalIsDisplayed)
            {
                journalScreen.DisplayJournalScreen();
            }
            else
            {
                journalScreen.ExitJournalScreen();
            }
        }
    }


    private void TriggerPanel()
    {
        instructionPanelScreen.EnablePanel();
    }

    public void LoadLevelContext()
    {
        currentPlayerContext = SaveLoadManager.LoadPlayerData();
        OnLevelContextUpdated(currentPlayerContext);
    }

    public void SaveLevelContext()
    {
        SaveLoadManager.SavePlayerData(playerController.transform,PhaseManager.currentPhase.phaseIndex);
        OnPlayerDataSaved(currentPlayerContext);
    }
    

    
    private void OnInstructionPanelEnd()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gamePausing.PlayGame();
    }

    private void OnDetectiveRemarkStart(Clue clue)
    {
        isBusy = true;
    }

    private void OnDetectiveRemarkEnd(Clue clue)
    {
        isBusy = false;
    }

    private void OnFinishedPropInteracting(GameObject go,Props prop)
    {
        if (!prop.isNestedAction)
        {
            isInteracting = false;
        }
    }
    
    private void OnStartedPropInteracting(GameObject go,Props prop)
    {
        isInteracting = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnPhaseUpdate(Phase phase)
    {
        StartCoroutine(SetPlayer());
    }

    private IEnumerator SetPlayer()
    {
        if (CheckForNoPlay())
        {
            playerController.enabled = false;
        }
        else
        {
            yield return new WaitUntil(() => isBusy ==false);
            playerController.enabled = true;
        } 
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