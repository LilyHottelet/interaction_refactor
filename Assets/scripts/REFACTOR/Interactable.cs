using System;
using UnityEngine;


public enum InteractableType
{
    Look,
    Use,
    Talk
}


public abstract class Interactable : MonoBehaviour
{
    public InteractableType type;

    public abstract void PlayInteraction();

    public delegate void EndedInteractionEventHandler();
    public static event EndedInteractionEventHandler EndedInteraction;
    protected virtual void OnEndedInteraction()
    {
        EndedInteraction?.Invoke();
    }
}