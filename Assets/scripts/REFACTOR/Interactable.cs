using System;
using UnityEngine;


public enum InteractableType
{
    Prop,
    NPC,
    Door
}

public class Interactable : MonoBehaviour
{
    public InteractableType type;

    public void PlayInteraction()
    {
        throw new NotImplementedException();
    }

    public delegate void EndedInteractionEventHandler();
    public static event EndedInteractionEventHandler EndedInteraction;
    protected virtual void OnEndedInteraction()
    {
        EndedInteraction?.Invoke();
    }
}