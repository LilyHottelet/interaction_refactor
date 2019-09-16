using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoorsSound : MonoBehaviour
{

    public AudioSource soundSource;
    public AudioClip elevatorDoorSoundClip;
    public AudioClip elevatorDingSoundClip;
    private Elevator elevatorManager;

    private void Awake()
    {
        elevatorManager = GameObject.Find("ELEVATORBOX").GetComponent<Elevator>();
    }


    public void PlayDoorSound()
    {
        soundSource.PlayOneShot(elevatorDoorSoundClip);	
    }

    public void SetClosed()
    {
        elevatorManager.isClosed = true;
    }
	
    public void SetOpened()
    {
        elevatorManager.isClosed = false;
    }

    public void PlayDing()
    {
        soundSource.PlayOneShot(elevatorDingSoundClip);
    }
}