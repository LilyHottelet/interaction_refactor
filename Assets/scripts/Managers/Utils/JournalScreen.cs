using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
 

public class JournalScreen : ManagerEvents
{
    private GamePausing gamePausing;
    public GameObject journalUI;
    public GameObject mainUI;
    public bool journalIsDisplayed = false;
    //public GameObject NoteSpread;
    public GameObject EvidenceSpread;
    public GameObject PeopleSpread;
    public Image EvidenceSprite;
    public TextMeshProUGUI EvidenceDescription;

    public Button journalButtonPrefab;
    public Transform buttonParentEvidence;
    public Transform buttonParentPeople;

    public Image portraitImage;
    public TextMeshProUGUI portraitText;
    
    public int buttonHeight = 50;

    public GameObject TranscriptScreen;
    public Animator journalAnimator;
    public AudioClip displaySound;

    public Texture2D defaultCursor;
    

    private void Awake()
    {
        gamePausing = GameObject.Find("Managers").GetComponent<GamePausing>();
    }

    private void OnEnable()
    {
        ManagerEvents.DetectiveRemarkStarted += OnDetectiveRemarkStart;
        ManagerEvents.FinishedNpcInteraction += OnFinishedNpcInteracting;
    }

    private void OnDisable()
    {
        ManagerEvents.DetectiveRemarkStarted -= OnDetectiveRemarkStart;
        ManagerEvents.FinishedNpcInteraction -= OnFinishedNpcInteracting;
    }

    public void DisplayJournalScreen()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.SetCursor(defaultCursor, Vector2.zero,CursorMode.Auto);
        Cursor.visible = true;
        OnUIPanelDisplayed(displaySound);
        journalUI.SetActive(true);
        mainUI.SetActive(false);
        DisplayEvidence();
        gamePausing.PauseGame();
        journalIsDisplayed = true;
    }

    public void ExitJournalScreen()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(AnimateOutAndDisable());
    }

    private IEnumerator AnimateOutAndDisable()
    {
        journalAnimator.SetTrigger("play_out");
        yield return new WaitForSeconds(0.5f);
        journalUI.SetActive(false);
        mainUI.SetActive(true);
        gamePausing.PlayGame();
        journalIsDisplayed = false;
    }

    private void OnDetectiveRemarkStart(Clue clue)
    {
        if (clue != null && clue.isJournalEntry)
        {
            switch (clue.type)
            {
                case Clue.TypeOfClue.Evidence:
                    SetEventTrigger(InstantiateButton(clue.title,buttonParentEvidence,clue));
                    DisplayInfo(clue);
                    break;
                
            }  
        }
    }
    
    private void OnFinishedNpcInteracting(NPCs npc,GameObject go)
    {
        if (npc.portrait!=null && !npc.hasBeenMet)
        {
            SetEventTrigger(InstantiateButton(npc.portrait.title,buttonParentPeople,npc.portrait));
            DisplayInfo(npc.portrait); 
        }

        foreach (var clue in npc.givenClues)
        {
            if (!clue.hasBeenFound)
            {
                SetEventTrigger(InstantiateButton(clue.title,buttonParentEvidence,clue));
                DisplayInfo(clue);
            }
        }
    }
    

//    public void DisplayNotes()
//    {
//        EvidenceSpread.SetActive(false);
//        PeopleSpread.SetActive(false);
//        NoteSpread.SetActive(true);
//    }

    public void DisplayEvidence()
    {
        PeopleSpread.SetActive(false);
        //NoteSpread.SetActive(false);
        EvidenceSpread.SetActive(true);
    }
    
    public void DisplayPeople()
    {
        //NoteSpread.SetActive(false);
        EvidenceSpread.SetActive(false);
        PeopleSpread.SetActive(true);
    }

    public void DisplayInfo(Clue clue = null)
    {
        Clue targetClue;
        if (clue != null)
        {
            targetClue = clue;
        }
        else
        {
            GameObject target = EventSystem.current.currentSelectedGameObject;
            targetClue = target.GetComponent<ClueComponent>().clue; 
        }

        switch (targetClue.type)
        {
            case Clue.TypeOfClue.Evidence:
                EvidenceSprite.sprite = targetClue.noteBookSprite;
                EvidenceDescription.text = targetClue.flavourText;
                break;
            case Clue.TypeOfClue.Portait:
                portraitImage.sprite = targetClue.noteBookSprite;
                portraitText.text = targetClue.flavourText;
                break;
        }
    }

    private Button InstantiateButton(string title,Transform buttonParent,Clue assignedClue)
    {
        Button newButton = Instantiate(journalButtonPrefab, buttonParent);
        newButton.transform.SetParent(buttonParent,false);
        int offset = buttonParent.childCount * buttonHeight;
        newButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, -offset);
        newButton.GetComponent<ClueComponent>().clue = assignedClue;
        newButton.GetComponentInChildren<TextMeshProUGUI>().text = title;
        return newButton;
    }

    private void SetEventTrigger(Button button)
    {
        EventTrigger trigger = button.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener( (eventData) => { DisplayInfo(); } );
        trigger.triggers.Add(entry);
    }

    public void DisplayTranscript()
    {
        TranscriptScreen.SetActive(true);
        //@TODO: ADD A METHOD TO DISPLAY THE APPROPRIATE DIALOGUE
    }
        
    
}