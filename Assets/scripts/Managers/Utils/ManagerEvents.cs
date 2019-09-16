using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ManagerEvents : MonoBehaviour {

    public delegate void FinishedPropInteractionEventHandler(GameObject go,Props prop);
    public static event FinishedPropInteractionEventHandler FinishedPropInteraction;
    protected virtual void OnFinishedPropInteraction(GameObject go,Props prop)
    {
        if (FinishedPropInteraction!=null)
        {
            FinishedPropInteraction(go,prop);
        }
    }
	
    public delegate void StartedNpcInteractionEventHandler(NPCs npc,GameObject go,string passageToLoad);
    public static event StartedNpcInteractionEventHandler StartedNpcInteraction;
    protected virtual void OnStartedNpcInteraction(NPCs npc,GameObject go,string passageToLoad)
    {
        if (StartedNpcInteraction!=null)
        {
            StartedNpcInteraction(npc,go,passageToLoad);
        }
    }
    
    public delegate void TriedForDialogueEventHandler(NPCs npc,GameObject go);
    public static event TriedForDialogueEventHandler TriedForDialogue;
    protected virtual void OnTriedForDialogue(NPCs npc,GameObject go)
    {
        if (TriedForDialogue!=null)
        {
            TriedForDialogue(npc,go);
        }
    }
	

    public delegate void StartedPropInteractionEventHandler(GameObject go,Props prop);
    public static event StartedPropInteractionEventHandler StartedPropInteraction;
    protected virtual void OnStartedPropInteraction(GameObject go,Props prop)
    {
        if (StartedPropInteraction!=null)
        {
            StartedPropInteraction(go,prop);
        }
    }

	
    public delegate void FinishedNpcInteractionEventHandler(NPCs npc, GameObject go);
    public static event FinishedNpcInteractionEventHandler FinishedNpcInteraction;
    protected virtual void OnFinishedNpcInteraction(NPCs npc,GameObject go)
    {
        if (FinishedNpcInteraction!=null)
        {
            FinishedNpcInteraction(npc,go);
        }
    }
	
    public delegate void UpdatedPhaseEventHandler(Phase phase);
    public static event UpdatedPhaseEventHandler UpdatedPhase;
    protected virtual void OnUpdatedPhase(Phase phase)
    {
        if (UpdatedPhase!=null)
        {
            UpdatedPhase(phase);
        }
    }
    
    public delegate void DetectiveRemarkStartedEventHandler(Clue clue);
    public static event DetectiveRemarkStartedEventHandler DetectiveRemarkStarted;
    protected virtual void OnDetectiveRemarkStarted(Clue clue)
    {
        if (DetectiveRemarkStarted!=null)
        {
            DetectiveRemarkStarted(clue);
        }
    }


    public delegate void DetectiveRemarkEndedEventHandler(Clue clue);
    public static event DetectiveRemarkEndedEventHandler DetectiveRemarkEnded;
    protected virtual void OnDetectiveRemarkEnded(Clue clue)
    {
        if (DetectiveRemarkEnded!=null)
        {
            DetectiveRemarkEnded(clue);
        }
    }
    
    public delegate void UIPanelDisplayedEventHandler(AudioClip soundToPlay);
    public static event UIPanelDisplayedEventHandler UIPanelDisplayed;
    protected virtual void OnUIPanelDisplayed(AudioClip soundToPlay)
    {
        if (UIPanelDisplayed!=null)
        {
            UIPanelDisplayed(soundToPlay);
        }
    }
    
    public delegate void LevelContextUpdatedEventHandler(PlayerData playerData);
    public static event LevelContextUpdatedEventHandler LevelContextUpdated;
    protected virtual void OnLevelContextUpdated(PlayerData playerData)
    {
        if (LevelContextUpdated!=null)
        {
            LevelContextUpdated(playerData);
        }
    }
    
    public delegate void PlayerDataSavedEventHandler(PlayerData playerData);
    public static event PlayerDataSavedEventHandler PlayerDataSaved;
    protected virtual void OnPlayerDataSaved(PlayerData playerData)
    {
        if (PlayerDataSaved!=null)
        {
            PlayerDataSaved(playerData);
        }
    }
    
    public delegate void DoorOpenedEventHandler(Door door);
    public static event DoorOpenedEventHandler DoorOpened;
    protected virtual void OnDoorOpened(Door door)
    {
        if (DoorOpened!=null)
        {
            DoorOpened(door);
        }
    }
	
    public delegate void ChangedRoomEventHandler(Door door);
    public static event ChangedRoomEventHandler ChangedRoom;
    protected virtual void OnChangedRoom(Door door)
    {
        if (ChangedRoom!=null)
        {
            ChangedRoom(door);
        }
    }
    
    public delegate void CalledElevatorEventHandler();
    public static event CalledElevatorEventHandler CalledElevator;
    protected virtual void OnCalledElevator ()
    {
        if (CalledElevator!=null)
        {
            CalledElevator();
        }
    }
    
    public delegate void ChangedFloorEventHandler(ElevatorButton button);
    public static event ChangedFloorEventHandler ChangedFloor;
    protected virtual void OnChangedFloor(ElevatorButton button)
    {
        if (ChangedFloor!=null)
        {
            ChangedFloor(button);
        }
    }
    
    public delegate void MustLoadSceneEventHandler(Room roomToLoad);
    public static event MustLoadSceneEventHandler MustLoadScene;
    protected virtual void OnMustLoadScene(Room roomToLoad)
    {
        if (MustLoadScene!=null)
        {
            MustLoadScene(roomToLoad);
        }
    }

    public delegate void FinishedCinematicEventHandler(Clue progressClue);

    public static event FinishedCinematicEventHandler FinishedCinematic;

    protected virtual void OnFinishedCinematic(Clue progressClue)
    {
        if (FinishedCinematic!=null)
        {
            FinishedCinematic(progressClue);
        }
    }

    public delegate void LoadedEnvironmentEventHandler();

    public static event LoadedEnvironmentEventHandler LoadedEnvironment;

    protected virtual void OnLoadedEnvironment()
    {
        if (LoadedEnvironment!=null)
        {
            LoadedEnvironment();
        }
    }

}