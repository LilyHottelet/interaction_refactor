using System.Collections;
using UnityEngine;
using Ink.Runtime;
using TMPro;

using UnityEngine.UI;
using DigitalRuby.Tween;


public class DialogueManager : ManagerEvents
{
    public Canvas canvas;
    public Button buttonPrefab;
    public TextMeshProUGUI commonTextBox;
    public GameObject choicesBox;
    private Story story;
    private bool isInDialogue;
    private string[] stringsToRemove  = {"NPC:","PLAYER:"};
    private AudioSource source;
    private NPCs currentNPC;
    private GameObject currentNPCGo;
    public bool FinishedTalking = true;
    private Fading fading;
    Color transparentColor = new Color(255F, 255F, 255F, 0F);
    Color opaqueColor = new Color(255F, 255F, 255F, 1F);
    private float lerpSpeed = 1f;
    private Text answerText;
    private bool hasSetHarold = false;
    public NPCs haroldNpcAsset;
    public Texture2D defaultCursor;
    //SAVE
    private string savedNPCTestJSON;
    private bool dialogueRanOut;

    [Header("GUN INTERACTION")]
    public Camera gunCam;
    public GameObject gunGO;
    private PropRotationHandler rotationHandler;
    private bool gotGun;
    public AudioClip interactionSound;
    
    

    private void Awake()
    {
        rotationHandler = GameObject.Find("Input").GetComponent<PropRotationHandler>();
        source = GetComponent<AudioSource>();
        fading = GameObject.Find("fade").GetComponent<Fading>();
    }

    private void OnEnable()
    {
        ManagerEvents.StartedNpcInteraction += OnStartedNpcInteracting;
        ManagerEvents.FinishedNpcInteraction += OnFinishedNpcInteracting;
        ManagerEvents.TriedForDialogue += OnTryDialogue;
        ManagerEvents.UpdatedPhase += OnPhaseUpdate;
    }

    private void OnDisable()
    {
        ManagerEvents.StartedNpcInteraction -= OnStartedNpcInteracting;
        ManagerEvents.FinishedNpcInteraction -= OnFinishedNpcInteracting;
        ManagerEvents.TriedForDialogue -= OnTryDialogue;
        ManagerEvents.UpdatedPhase -= OnPhaseUpdate;
    }

    private void OnPhaseUpdate(Phase phase)
    {
        if (phase.phaseIndex > 2 && !hasSetHarold)
        {
            currentNPC = haroldNpcAsset;
            story = new Story(currentNPC.dialogue.text);
            story.variablesState["nextActivePassage"] = "04_meeting_harold";
            currentNPC.savedDialogueState = story.state.ToJson();
            currentNPC.hasBeenMet = true;
            Debug.Log(story.variablesState["nextActivePassage"]);
            hasSetHarold = true;
        }
    }

    private void OnTryDialogue(NPCs npc,GameObject go)
    {
        if (npc.hasBeenMet)
        {
            if (npc.savedDialogueState!="")
            {
                story = new Story(npc.dialogue.text);
                story.state.LoadJson(npc.savedDialogueState);
            }
            else
            {
                Debug.Log("Npc has been met but there is no save");
                return;
            }
        }
        else
        {
            story = new Story(npc.dialogue.text);
            
        }

        string currentPhase = PhaseManager.currentPhase.name;
        string passageToLoad = (string) story.variablesState["nextActivePassage"];
        string currentPassage = (string) story.variablesState["currentPhase"];
        if (currentPassage == currentPhase && !story.canContinue)
        {
            Debug.Log("no dialogue to load right now because out");
        }
        else if (passageToLoad == currentPhase)
        {
            StartCoroutine(FadingNpc(npc,go,true,passageToLoad));
        }
        else
        {
            TriggerRandomSentence(go.GetComponent<NPCComponent>());
            Debug.Log("no dialogue to load because not in right phase. currentPhase : "+currentPhase+" nextActivePassage : "+passageToLoad);
        }
    }

    private void OnStartedNpcInteracting(NPCs npc,GameObject go,string passageToLoad)
    {
        if (!npc.hasBeenMet)
        {
            npc.hasBeenMet = true;
            //EditorUtility.SetDirty(npc);

        }
        canvas.enabled = true;
        currentNPC = npc;
        currentNPCGo = go;
        isInDialogue = true;
        story.ChoosePathString(passageToLoad);
        Debug.Log(passageToLoad);
        RefreshView();
        Cursor.SetCursor(defaultCursor,Vector2.zero,CursorMode.Auto);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (npc.name != "Phone")
        {
            go.GetComponent<Animator>().SetTrigger("dialog");
        }
       
    }

    private void OnFinishedNpcInteracting(NPCs npc, GameObject go)
    {
        if (npc.name != "Phone")
        {
            go.GetComponent<Animator>().SetTrigger("default");
        }
       
    }

    private void Update()
    {
        if (isInDialogue)
        {
            RefreshView();
        }
    }

    private void RefreshView()
    {
        if (FinishedTalking && story.canContinue)
        {
            string textLine = story.Continue();
            //Chose the right display box
            TextMeshProUGUI textBox = commonTextBox;
            
            //Check for tags
            if (story.currentTags.Count > 0)
            {
                PlayAudio(story.currentTags[0]);
                if (story.currentTags.Count >= 2)
                {
                    if (story.currentTags[1]!="none" && currentNPC.name != "Phone")
                    {
                        PlayAnimation(story.currentTags[1]);
                    }
                    if (story.currentTags.Count >= 3)
                    {
                        if (story.currentTags[2]!="none")
                        {
                            CheckForClue(story.currentTags[2]);
                        }
                        if (story.currentTags.Count >= 4 && story.currentTags[3] == "show_gun")
                        {
                            ShowGun();
                            
                            
                        }
                    }
                }
            }

            textLine = textLine.Replace(stringsToRemove[0], "");
            textBox.text = textLine;
            RefreshView();
        }
        else if (FinishedTalking && !story.canContinue)
        {
            isInDialogue = false;
            if (story.currentChoices.Count>0)
            {
                // Display all the choices, if there are any!
                if(story.currentChoices.Count > 0)
                {
                    //StartCoroutine(TestDelayLoop());
                    for (int i = 0; i < story.currentChoices.Count; i++) {
                        Choice choice = story.currentChoices [i];
                        Button button = CreateChoiceButton (choice.text.Trim (),i);
                        TweenAlpha(choicesBox);
                        Debug.Log("no if statement");
                        // Tell the button what to do when we press it
                        button.onClick.AddListener (delegate {
                            OnClickChoiceButton (choice);
                        });
                    }
                }  
            }
            else
            {
                EndDialogue();
            }

        }
    }

    public void ResetView()
    {
        int childCount = choicesBox.transform.childCount;
        for (int i = childCount - 1; i >= 0; --i) {
            GameObject.Destroy (choicesBox.transform.GetChild (i).gameObject);
        }
        
    }

    private void PlayAudio(string audioname)
    {
        FinishedTalking = false;
        AudioClip audio = Resources.Load<AudioClip>("voices/" + audioname);
        source.PlayOneShot(audio);
        StartCoroutine(WaitForAudioEnd(audio.length));
    }

    private void PlayAnimation(string trigger)
    {
        currentNPCGo.GetComponent<Animator>().SetTrigger(trigger);
    }

    private IEnumerator WaitForAudioEnd(float audioLength)
    {
        yield return new WaitForSeconds(audioLength);
        FinishedTalking = true;
    }

    private Button CreateChoiceButton(string textContent,int increment)
    {
        Button tempBtn = Instantiate(buttonPrefab) as Button;
        tempBtn.GetComponentInChildren<TextMeshProUGUI> ().text = textContent;
        tempBtn.gameObject.name = "button" + increment.ToString();
        tempBtn.transform.SetParent (choicesBox.transform, false);
        HorizontalLayoutGroup layoutGroup = tempBtn.GetComponent <HorizontalLayoutGroup> ();
        layoutGroup.childForceExpandHeight = false;
        return tempBtn;
    }
    
    void OnClickChoiceButton (Choice choice) {
        story.ChooseChoiceIndex (choice.index);
        isInDialogue = true;
        ResetView();
    }

    public void EndDialogue()
    {
        currentNPC.savedDialogueState = story.state.ToJson();
        StartCoroutine(FadingNpc(currentNPC, currentNPCGo, false));
        canvas.enabled = false;
        isInDialogue = false;
        
        ResetView();
        if (gotGun)
        {
            gunCam.enabled = false;
            gunGO.SetActive(false);
            gotGun = false;
        }
        //EditorUtility.SetDirty(currentNPC);
    }

    private void CheckForClue(string clueID)
    {
        Clue receivedClue = Dictionaries.ClueDictionary[clueID];
        currentNPC.givenClues.Add(receivedClue);
        //EditorUtility.SetDirty(currentNPC);
    }

    private IEnumerator FadingNpc(NPCs npc, GameObject go, bool isEntering,string passage ="")
    {
        fading.StartFadeOut();
        yield return new WaitForSeconds(0.5f);
        if (isEntering)
        {
            OnStartedNpcInteraction(npc,go,passage);   
        }
        else
        {
            OnFinishedNpcInteraction(currentNPC,currentNPCGo);
        }
    }
    
    private void TweenAlpha(GameObject go)
    {
        TextMeshProUGUI[] answerText = go.GetComponentsInChildren<TextMeshProUGUI>();
       
        go.Tween(null,transparentColor,opaqueColor,0.8f, TweenScaleFunctions.CubicEaseInOut, (t) =>
        {
            // progress
            answerText[0].color = t.CurrentValue;
            if (answerText.Length > 1)
            {
                answerText[1].color = t.CurrentValue; 
            }
           
        }, (t) =>
        {
            // completion
        });
    }

    private  IEnumerator TestTween(GameObject go)
    {
        float t2 = 0.0f;
        answerText = go.GetComponentInChildren<Text>();
        while (t2< 1f)
        {
            t2 += Time.deltaTime * (Time.timeScale / lerpSpeed);
            answerText.color = Color.Lerp(transparentColor, opaqueColor, t2);
            yield return 0;
        }
        
    }

    private IEnumerator TestDelayLoop()
    {
        for (int i = 0; i < story.currentChoices.Count; i++) {
            Choice choice = story.currentChoices [i];
            Button button = CreateChoiceButton (choice.text.Trim (),i);
            TestTween(button.gameObject);
            //TweenAlpha(button.gameObject,true);
            // Tell the button what to do when we press it
            button.onClick.AddListener (delegate {
                OnClickChoiceButton (choice);
            });
            yield return new WaitForSeconds(0);
        }
    }


    private void ShowGun()
    {
        source.PlayOneShot(interactionSound);
        gunCam.enabled = true;
        gunGO.SetActive(true);
        rotationHandler.currentTargetGo = gunGO;
        rotationHandler.enabled = true;
        gotGun = true;

    }


    private void TriggerRandomSentence(NPCComponent npcData)
    {
        Clue randomRemark = npcData.NoDialogueRemarks[GetRandomInt(npcData.NoDialogueRemarks.Length)];
        OnDetectiveRemarkStarted(randomRemark);
    }

    private int GetRandomInt(int maxValue)
    {
        int randomInt = Random.Range(0, maxValue);
        return randomInt;
    }
}