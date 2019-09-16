using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InteractionHandler : ManagerEvents
{
    private Ray ray;
    private RaycastHit hit;
    public LayerMask interactionLayerMask;
    private Interactable currentInteractable;

    private bool interactionNotified;
    private bool exitInteractionNotified;

    public bool isInteracting = false;
    private PropRotationHandler rotationHandler;
    private ExitInputHandler exitInputHandler;
    private Fading fading;
    private Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);

    public Image interactionIcon;
    public Camera mainCam;
    public Props currentTarget;
    public GameObject currentTargetGo;
    private NPCs currentNpc;

    public Sprite cursorLook;
    public Sprite cursorTalk;
    public Sprite cursorUse;

    private void Start()
    {
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Awake()
    {
        //ManagerEvents.DetectiveRemarkEnded += OnDetectiveRemarkEnd;
        //ManagerEvents.FinishedPropInteraction += OnFinishedPropInteracting;
    }

    private void Update()
    {
        // Cast a ray to the center of the screen
        if (IsHittingObject())
        {
            currentInteractable = hit.collider.GetComponent<Interactable>();
            UIManager.Instance.InitiateCursor(currentInteractable);
            ManageInput();
        }
        else
        {
            interactionNotified = false;
            if (!exitInteractionNotified)
            {
                exitInteractionNotified = true;
                UIManager.Instance.KillIcon();
            }
        }
    }

    private void ManageInput()
    {
        ////Dealing with input here, should be done in separate method. We should also know what were dealing with already
        ///Should sub to the object's end event here ? 
        if (Input.GetKeyDown(KeyCode.Mouse0) && !GameManager.isBusy)
        {
            currentInteractable.PlayInteraction();
            isInteracting = true;
        }
    }

    private bool IsHittingObject()
    {
        //change to a sphere cast ?
        ray = mainCam.ScreenPointToRay(screenCenterPoint);
        return Physics.Raycast(ray, out hit, 1.8f, interactionLayerMask) && hit.transform.gameObject.CompareTag("Interactable");
    }


    //The object itself should disable its collider when found
    //private bool CheckForHasBeenFound(Clue clue)
    //{
    //    if (clue != null && clue.hasBeenFound)
    //    {
    //        Debug.Log("has been found");
    //        return true;
    //    }

    //    return false;
    //}

    private void SetRotationHandler()
    {
        rotationHandler.currentTargetGo = currentTargetGo;
        rotationHandler.enabled = true;
    }

    private void OnFinishedPropInteracting(GameObject go, Props prop)
    {
        rotationHandler.enabled = false;
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