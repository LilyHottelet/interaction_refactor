using System.Collections;
using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroCinematic : ManagerEvents
{
    public TextAsset textContent;
    private Story story;
    public TextMeshProUGUI textBox;
    private bool FinishedTalking = true;
    private AudioSource source;
    public Room nextSceneToLoad;
    private bool isInCinematic = false;
    public Clue cinematicClue;
    

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine(DelayCinematic());
    }
    
    private void Start()
    {
        story = new Story(textContent.text);
    }
    private void Update()
    {
        if (isInCinematic)
        {
            RefreshView();
        }

    }
    private void RefreshView()
    {
        if (FinishedTalking && story.canContinue)
        {
            string textLine = story.Continue();

            //Check for tags
            if (story.currentTags.Count > 0)
            {
                PlayAudio(story.currentTags[0]);
            }

            textBox.text = textLine;
            RefreshView();
        }

        if (FinishedTalking && !story.canContinue)
        {
            isInCinematic = false;
            OnMustLoadScene(nextSceneToLoad);
            OnFinishedCinematic(cinematicClue);
            
        }
    }
    
    private void PlayAudio(string audioname)
    {
        FinishedTalking = false;
        AudioClip audio = Resources.Load<AudioClip>("voices/" + audioname);
        source.PlayOneShot(audio);
        StartCoroutine(WaitForAudioEnd(audio.length));
    }
    
    private IEnumerator WaitForAudioEnd(float audioLength)
    {
        yield return new WaitForSeconds(audioLength);
        FinishedTalking = true;
    }

    private IEnumerator DelayCinematic()
    {
        yield return new WaitForSeconds(1.5f);
        isInCinematic = true;
    }

}