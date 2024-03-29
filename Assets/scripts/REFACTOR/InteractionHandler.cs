﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InteractionHandler : ManagerEvents
{
    private Ray ray;
    private RaycastHit hit;
    private bool interactionNotified;
    private bool exitInteractionNotified;

    public bool isInteracting = false;
    private PropRotationHandler rotationHandler;
    private ExitInputHandler exitInputHandler;
    private Fading fading;
    private Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);

    public Image interactionIcon;
    public Camera mainCam;
    public Camera raycastCam;
    public Props currentTarget;
    public GameObject currentTargetGo;
    private NPCs currentNpc;

    public Sprite cursorLook;
    public Sprite cursorTalk;
    public Sprite cursorUse;

    private void Start()
    {
        rotationHandler = GameObject.Find("Input").GetComponent<PropRotationHandler>();
        fading = GameObject.Find("fade").GetComponent<Fading>();
        exitInputHandler = GameObject.Find("Input").GetComponent<ExitInputHandler>();
        SetRayCastingCamera(mainCam);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Awake()
    {
        ManagerEvents.DetectiveRemarkEnded += OnDetectiveRemarkEnd;
        ManagerEvents.FinishedPropInteraction += OnFinishedPropInteracting;
    }

    private void Update()
    {
        // Cast a ray to the center of the screen
        ray = raycastCam.ScreenPointToRay(screenCenterPoint);

        //Compare tag - can be improved with a layer check
        if (Physics.Raycast(ray, out hit, 1.8f) && !hit.transform.gameObject.CompareTag("Untagged"))
        {
            //check for the prop tag
            if (hit.transform.gameObject.CompareTag("InteractableProp"))
            {
                //dont do anything if its been found - disable collider 
                if (CheckForHasBeenFound(hit.transform.GetComponent<PropComponent>().prop.clue))
                    return;
                //avoid multiple getcomp
                Transform iconTransform = hit.transform.GetComponent<PropComponent>().iconSpawnPosition;
                // delegate the responsibility of checking what it is to a subscript
                if (hit.transform.GetComponent<PropComponent>().prop.type == Props.TypeOfProp.Door || hit.transform.GetComponent<PropComponent>().prop.type == Props.TypeOfProp.Kinetoscope)
                {
                    //Create a singleton ui manager to handle this
                    //the sprites should be the responsibility of UI, not the detector
                    InitiateCursor(iconTransform, cursorUse);
                }
                else
                {
                    InitiateCursor(iconTransform, cursorLook);
                }


            }
            //They should basically have the same class and have a enum type 
            if (hit.transform.gameObject.CompareTag("InteractableNPC"))
            {
                Transform iconTransform = hit.transform.GetComponent<NPCComponent>().iconSpawnPosition;
                InitiateCursor(iconTransform, cursorTalk);
            }

            if (hit.transform.gameObject.CompareTag("Elevator"))
            {
                Transform iconTransform = hit.transform.GetComponent<IconPosition>().iconSpawnPosition;
                InitiateCursor(iconTransform, cursorUse);
            }
            if (hit.transform.gameObject.CompareTag("ElevatorButton"))
            {
                Transform iconTransform = hit.transform.GetComponent<ElevatorButton>().iconSpawnPosition;
                InitiateCursor(iconTransform, cursorUse);
            }
            //Dealing with input here, should be done in separate method. We should also know what were dealing with already
            if (Input.GetKeyDown(KeyCode.Mouse0) && !GameManager.isBusy)
            {
                if (hit.transform.gameObject.CompareTag("InteractableNPC"))
                {
                    currentNpc = hit.collider.GetComponent<NPCComponent>().npcAsset;
                    OnTriedForDialogue(currentNpc, hit.collider.gameObject);
                }

                if (hit.transform.gameObject.CompareTag("InteractableProp"))
                {
                    currentTarget = hit.collider.GetComponent<PropComponent>().prop;
                    currentTargetGo = hit.collider.gameObject;
                    TriggerPropEvent(currentTarget.type);
                }

                if (hit.transform.gameObject.CompareTag("Elevator"))
                {
                    OnCalledElevator();
                }

                if (hit.transform.gameObject.CompareTag("ElevatorButton"))
                {
                    OnChangedFloor(hit.transform.gameObject.GetComponent<ElevatorButton>());
                    Debug.Log("test");
                }

                isInteracting = true;
            }
        }
        else
        {
            interactionNotified = false;
            if (!exitInteractionNotified)
            {
                exitInteractionNotified = true;
                KillIcon();
            }
        }
    }

    private bool CheckForHasBeenFound(Clue clue)
    {
        if (clue != null && clue.hasBeenFound)
        {
            Debug.Log("has been found");
            return true;
        }

        return false;
    }

    private void InitiateCursor(Transform cursorPosition, Sprite cursorSprite)
    {
        interactionIcon.transform.position = raycastCam.WorldToScreenPoint(cursorPosition.position);
        exitInteractionNotified = false;
        if (!interactionNotified)
        {
            interactionNotified = true;
            DisplayIcon(cursorSprite);
        }
    }

    public void TriggerPropEvent(Props.TypeOfProp propType)
    {
        switch (propType)
        {
            case Props.TypeOfProp.Inspect:
                OnTriggeredInteraction();
                if (currentTarget.clue == null || currentTarget.clue.detectiveRemarkAudio == null)
                {
                    SetExitInputHandler(true);
                }
                else
                {
                    exitInputHandler.enabled = false;
                }
                StartCoroutine(FadingProp());
                if (!currentTarget.hasNextAction)
                {
                    SetRotationHandler();
                }
                break;
            case Props.TypeOfProp.Read:
                OnTriggeredInteraction();
                if (currentTarget.clue == null || currentTarget.clue.detectiveRemarkAudio == null)
                {
                    SetExitInputHandler(true);
                }
                else
                {
                    exitInputHandler.enabled = false;
                }
                StartCoroutine(FadingProp());
                break;
            case Props.TypeOfProp.Kinetoscope:
                OnTriggeredInteraction();
                SetExitInputHandler(true);
                StartCoroutine(FadingProp());
                break;
            case Props.TypeOfProp.Look:
                if (currentTarget.clue == null || currentTarget.clue.detectiveRemarkAudio == null)
                {
                    SetExitInputHandler(true);
                }
                else
                {
                    exitInputHandler.enabled = false;
                }
                KillIcon();
                OnStartedPropInteraction(currentTargetGo, currentTarget);
                break;
            case Props.TypeOfProp.Sound:
            case Props.TypeOfProp.Reaction:
            case Props.TypeOfProp.Door:

                OnStartedPropInteraction(currentTargetGo, currentTarget);
                break;

            case Props.TypeOfProp.Elevator:
                SetExitInputHandler(true);
                OnStartedPropInteraction(currentTargetGo, currentTarget);
                break;
        }
    }

    private IEnumerator FadingProp()
    {
        fading.StartFadeOut();
        yield return new WaitForSeconds(0.5f);
        OnStartedPropInteraction(currentTargetGo, currentTarget);

    }

    //move to a ui script
    private void DisplayIcon(Sprite iconSprite)
    {
        Color temp = interactionIcon.color;
        temp.a = 255;
        interactionIcon.color = temp;
        interactionIcon.sprite = iconSprite;
    }

    //move to a ui script
    private void KillIcon()
    {
        Color temp = interactionIcon.color;
        temp.a = 0f;
        interactionIcon.color = temp;
        interactionIcon.sprite = null;
    }

    //exit input handler should be registered himself whatever it is
    private void SetExitInputHandler(bool isProp)
    {
        exitInputHandler.currentTarget = null;
        exitInputHandler.currentTargetGo = null;
        exitInputHandler.currentNpc = null;
        if (isProp)
        {
            exitInputHandler.currentTarget = currentTarget;
            exitInputHandler.currentTargetGo = currentTargetGo;
        }
        else
        {
            exitInputHandler.currentNpc = currentNpc;
        }

        exitInputHandler.enabled = true;
    }

    private void SetRotationHandler()
    {
        rotationHandler.currentTargetGo = currentTargetGo;
        rotationHandler.enabled = true;
    }

    //unnecessary
    public void SetRayCastingCamera(Camera newCamera)
    {
        raycastCam = newCamera;
    }

    private void OnFinishedPropInteracting(GameObject go, Props prop)
    {
        rotationHandler.enabled = false;
    }

    private void OnDetectiveRemarkEnd(Clue clue)
    {
        SetExitInputHandler(true);
    }

    public delegate void TriggeredInteractionEventHandler();
    public static event TriggeredInteractionEventHandler TriggeredInteraction;
    protected virtual void OnTriggeredInteraction()
    {
        if (TriggeredInteraction != null)
        {
            TriggeredInteraction();
        }
    }



}