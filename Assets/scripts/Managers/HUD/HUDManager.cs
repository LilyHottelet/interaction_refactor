using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DigitalRuby.Tween;

public class HUDManager : MonoBehaviour
{

    public GameObject UIMain;
    public GameObject UIProps;
    public GameObject UIRead;
    public GameObject UISave;
    public GameObject UIDialogue;
    public GameObject UIKinetoscope;
    public TextMeshProUGUI bottomText;
    public Image readingImage;
    private Fading fading;
    private Phase[] noPlayerPhases;

    private void Awake()
    {
        noPlayerPhases = GameObject.Find("Managers").GetComponent<GameManager>().noPlayerPhases;
        fading = GameObject.Find("fade").GetComponent<Fading>();
    }

    private void OnEnable()
    {
        ManagerEvents.StartedPropInteraction += OnStartedPropInteraction;
        ManagerEvents.StartedNpcInteraction += OnStartedNpcInteraction;
        ManagerEvents.FinishedPropInteraction += OnFinishedPropInteraction;
        ManagerEvents.FinishedNpcInteraction += OnFinishedNpcInteraction;
        ManagerEvents.DetectiveRemarkStarted += OnDetectiveRemarkStart;
        ManagerEvents.DetectiveRemarkEnded += OnDetectiveRemarkEnd;
        ManagerEvents.PlayerDataSaved += OnPlayerDataSave;
        ManagerEvents.UpdatedPhase += OnPhaseUpdate;

    }

    private void OnDisable()
    {
        ManagerEvents.StartedPropInteraction -= OnStartedPropInteraction;
        ManagerEvents.StartedNpcInteraction -= OnStartedNpcInteraction;
        ManagerEvents.FinishedPropInteraction -= OnFinishedPropInteraction;
        ManagerEvents.FinishedNpcInteraction -= OnFinishedNpcInteraction;
        ManagerEvents.DetectiveRemarkStarted -= OnDetectiveRemarkStart;
        ManagerEvents.DetectiveRemarkEnded -= OnDetectiveRemarkEnd;
        ManagerEvents.PlayerDataSaved -= OnPlayerDataSave;
        ManagerEvents.UpdatedPhase -= OnPhaseUpdate;

    }

    private void OnStartedPropInteraction(GameObject go,Props prop)
    {
        switch (prop.type)
        {
            case Props.TypeOfProp.Look:
                break;
            case Props.TypeOfProp.Read:
                readingImage.sprite = prop.readingMaterial;
                DisplayUI(UIRead);
                fading.StartFadeIn();
                break;
            case Props.TypeOfProp.Inspect:
                DisplayUI(UIProps);
                fading.StartFadeIn();
                break;
            case Props.TypeOfProp.Kinetoscope:
                DisplayUI(UIKinetoscope);
                fading.StartFadeIn();
                break;
        }
    }

    private void OnStartedNpcInteraction(NPCs npc,GameObject go,string passage)
    {
        DisplayUI(UIDialogue);
        fading.StartFadeIn();
       
    }

    private void OnFinishedPropInteraction(GameObject GO,Props prop)
    {       
        switch (prop.type)
        {
            case Props.TypeOfProp.Inspect:
                RestoreMainUI(UIProps);
                fading.StartFadeIn();
                break;
            case Props.TypeOfProp.Kinetoscope:
                RestoreMainUI(UIKinetoscope);
                fading.StartFadeIn();
                break;
            case Props.TypeOfProp.Read:
                RestoreMainUI(UIRead);
                fading.StartFadeIn();
                break;
        }
    }
    
    private void OnFinishedNpcInteraction(NPCs npc,GameObject go)
    {
        RestoreMainUI(UIDialogue);
        fading.StartFadeIn();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDetectiveRemarkStart(Clue clue)
    {
        if (clue!=null)
        {
            if (clue.ID == "irrelevant")
            {
                Clue randomClue =
                    Resources.Load<Clue>("IrrelevantRemarks/irrelevant_remark_" + RandomGenerator.currentRandomInt);
                DisplayText(randomClue.detectiveRemarkText);
                return;
            }
            DisplayText(clue.detectiveRemarkText);
        }
        
    }

    private void OnDetectiveRemarkEnd(Clue clue)
    {
        DisplayText("");
    }

    private void OnPlayerDataSave(PlayerData data)
    {
        StartCoroutine(WaitForSaveAnimation());
    }

    private void RestoreMainUI(GameObject UIToRemove)
    {
        UIToRemove.SetActive(false);
        UIMain.SetActive(true);
    }

    private void DisplayUI(GameObject UIToDisplay)
    {
        UIMain.SetActive(false);
        UIToDisplay.SetActive(true);
    }

    private void DisplayText(string reactionText)
    {
        bottomText.text = reactionText;
    }

    private string GetRandomLine()
    {
        //Get a random line from a data
        return "No data yet";
    }

    private IEnumerator RestoreMainUITest()
    {
        yield return new WaitForSeconds(fading.totalFadeDuration);
        UIMain.SetActive(true);
    }

    private IEnumerator WaitForSaveAnimation()
    {
        UISave.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        UISave.SetActive(false);
    }
    private void OnPhaseUpdate(Phase phase)
    {
        Invoke("SetMainUI",0.1f);
    }

    private void SetMainUI()
    {
        if (CheckForNoPlay())
        {
            UIMain.SetActive(false);
        }
        else
        {
            UIMain.SetActive(true);
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