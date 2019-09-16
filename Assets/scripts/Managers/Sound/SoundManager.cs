using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;


public class SoundManager : ManagerEvents {
    
    public AudioMixer mixer;
    public AudioSource mainSource;
    public AudioClip inspectSound;
    public AudioClip panelSound;
    public AudioClip debugSound;
    private GameObject currentTargetGo;
    public bool isPlayingRemarkSound;

    private void OnEnable()
    {
        ManagerEvents.StartedPropInteraction += OnStartedPropInteracting;
        ManagerEvents.StartedNpcInteraction += OnStartedNpcInteracting;
        ManagerEvents.FinishedNpcInteraction += OnFinishedNpcInteracting;
        ManagerEvents.DetectiveRemarkStarted += OnDetectiveRemarkStart;
        ManagerEvents.DoorOpened += OnDoorOpen;
        InteractionDetector.TriggeredInteraction += OnInteractionTriggered;
        ManagerEvents.UIPanelDisplayed += OnUIPanelDisplay;
        ManagerEvents.ChangedFloor += OnFloorChange;
    }

    private void OnDisable()
    {
        ManagerEvents.StartedPropInteraction -= OnStartedPropInteracting;
        ManagerEvents.StartedNpcInteraction -= OnStartedNpcInteracting;
        ManagerEvents.FinishedNpcInteraction -= OnFinishedNpcInteracting;
        ManagerEvents.DetectiveRemarkStarted -= OnDetectiveRemarkStart;
        ManagerEvents.DoorOpened -= OnDoorOpen;
        InteractionDetector.TriggeredInteraction -= OnInteractionTriggered;
        ManagerEvents.UIPanelDisplayed -= OnUIPanelDisplay;
        ManagerEvents.ChangedFloor -= OnFloorChange;
    }

    private void OnInteractionTriggered()
    {
        PlaySimpleSound(inspectSound,mainSource);
    }

    private void OnStartedPropInteracting(GameObject go,Props prop)
    {
        switch (prop.type)
        {
            case Props.TypeOfProp.Reaction:
            case Props.TypeOfProp.Kinetoscope:
                OnDetectiveRemarkStarted(prop.clue);
                break;
        }
    }

    private void OnStartedNpcInteracting(NPCs npc,GameObject go,string passage)
    {
        //@TODO ChangeMusicTo(Dialogue)
    }
    
    private void OnFinishedNpcInteracting(NPCs npc,GameObject go)
    {
        //@TODO Change music back to main ? 
    }

    private void OnDetectiveRemarkStart(Clue clue)
    {
        if (clue==null)
        {
            StartCoroutine(FireEndEvent(clue));
        }
        if (!isPlayingRemarkSound)
        {
            if (clue!=null && clue.detectiveRemarkAudio != null)
            {
                StartCoroutine(PlayDetectiveRemark(clue));
                isPlayingRemarkSound = true;

            }
            else if(clue!=null && clue.ID == "irrelevant")
            {
                StartCoroutine(PlayDetectiveRemark(GetRandomRemark()));
                isPlayingRemarkSound = true;
            }
            else
            {
                return;
            }
        }  
    }

    private void OnFloorChange(ElevatorButton button)
    {
        Debug.Log(button.sceneToLoad);
        PlaySimpleSound(Resources.Load<AudioClip>("elevatorTrip2"));
    }
    
    private void OnDoorOpen(Door door)
    {
        PlaySimpleSound(door.openingSound);
    }

    private void OnUIPanelDisplay(AudioClip soundToPlay)
    {
        mainSource.PlayOneShot(soundToPlay);
    }
    
    IEnumerator PlayInteractionSound(Props prop,AudioClip sound)
    {
        mainSource.PlayOneShot(sound);
        yield return new WaitForSeconds(sound.length);
        OnFinishedPropInteraction(null,prop);
    }

    IEnumerator PlayDetectiveRemark(Clue clue)
    {
        mainSource.PlayOneShot(clue.detectiveRemarkAudio);
        yield return new WaitForSeconds(clue.detectiveRemarkAudio.length);
        OnDetectiveRemarkEnded(clue);
        isPlayingRemarkSound = false;
    }

    private void PlaySimpleSound(AudioClip sound, AudioSource source = null)
    {
        if (source != null)
        {
            source.PlayOneShot(sound);
            
        }
        else
        {
            mainSource.PlayOneShot(sound);
        }
    }
    
    private Clue GetRandomRemark()
    {
        Clue randomRemark = Resources.Load<Clue>("IrrelevantRemarks/irrelevant_remark_" + RandomGenerator.currentRandomInt);
        return randomRemark;
    }

    public void PlayRandomSound(AudioClip[] sounds,AudioSource source = null)
    {
        int random = Random.Range(0, sounds.Length);
        
        if (source != null)
        {
            source.PlayOneShot(sounds[random]);
        }
        else
        {
            mainSource.PlayOneShot(sounds[random]);
        }
    }
    
    public void SetVolumeMaster(float level)
    {
        mixer.SetFloat ("masterVol", level);
    }
    public void SetVolumeMusic (float level)
    {
        mixer.SetFloat ("musicVol", level);
    }
    public void SetVolumeSfx (float level)
    {
        mixer.SetFloat ("sfxVol", level);
    }
    public void SetVolumeVoices (float level)
    {
        mixer.SetFloat ("voicesVol", level);
    }
    public void SetVolumeAmbiance(float level)
    {
        mixer.SetFloat ("ambianceVol", level);
    }
    public void SetVolumeUI(float level)
    {
        mixer.SetFloat ("uiVol", level);
    }

    private IEnumerator FireEndEvent(Clue clue)
    {
        yield return new WaitForSeconds(0.2f);
        OnDetectiveRemarkEnded(clue);
        
    }



}