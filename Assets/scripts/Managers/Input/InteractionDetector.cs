using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InteractionDetector : ManagerEvents
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
        ray = raycastCam.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out hit, 1.8f) && !hit.transform.gameObject.CompareTag("Untagged"))
        {
            //Debug.Log(hit.collider);
            if (hit.transform.gameObject.CompareTag("InteractableProp"))
            {
                if (CheckForHasBeenFound(hit.transform.GetComponent<PropComponent>().prop.clue))
                    return;
                Transform iconTransform = hit.transform.GetComponent<PropComponent>().iconSpawnPosition;
                if (hit.transform.GetComponent<PropComponent>().prop.type == Props.TypeOfProp.Door || hit.transform.GetComponent<PropComponent>().prop.type == Props.TypeOfProp.Kinetoscope)
                {
                    InitiateCursor(iconTransform,cursorUse);
                }
                else
                {
                    InitiateCursor(iconTransform,cursorLook);
                }
                
               
            }

            if (hit.transform.gameObject.CompareTag("InteractableNPC"))
            {
                Transform iconTransform = hit.transform.GetComponent<NPCComponent>().iconSpawnPosition;
                InitiateCursor(iconTransform,cursorTalk); 
            }

            if (hit.transform.gameObject.CompareTag("Elevator"))
            {
                Transform iconTransform = hit.transform.GetComponent<IconPosition>().iconSpawnPosition;
                InitiateCursor(iconTransform,cursorUse);
            }
            if (hit.transform.gameObject.CompareTag("ElevatorButton"))
            {
                Transform iconTransform = hit.transform.GetComponent<ElevatorButton>().iconSpawnPosition;
                InitiateCursor(iconTransform,cursorUse);
            }
            
            if (Input.GetKeyDown(KeyCode.Mouse0) )
            {
                Debug.Log("test");
                if (hit.transform.gameObject.CompareTag("InteractableNPC"))
                {
                    currentNpc = hit.collider.GetComponent<NPCComponent>().npcAsset;
                    OnTriedForDialogue(currentNpc,hit.collider.gameObject);
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
                if (currentTarget.clue==null || currentTarget.clue.detectiveRemarkAudio==null)
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
                if (currentTarget.clue==null || currentTarget.clue.detectiveRemarkAudio==null)
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
                if (currentTarget.clue==null || currentTarget.clue.detectiveRemarkAudio==null)
                {
                    SetExitInputHandler(true);
                }
                else
                {
                    exitInputHandler.enabled = false;
                }
                KillIcon();
                OnStartedPropInteraction(currentTargetGo,currentTarget);
                break;
            case Props.TypeOfProp.Sound:
            case Props.TypeOfProp.Reaction:
            case Props.TypeOfProp.Door:
                        
                OnStartedPropInteraction(currentTargetGo,currentTarget);
                break;
                            
            case Props.TypeOfProp.Elevator:
                SetExitInputHandler(true);
                OnStartedPropInteraction(currentTargetGo,currentTarget);
                break;
        }
    }

    private IEnumerator FadingProp()
    {
        fading.StartFadeOut();
        yield return new WaitForSeconds(0.5f);
        OnStartedPropInteraction(currentTargetGo,currentTarget);
		
    }

    private void DisplayIcon(Sprite iconSprite)
    {
        Color temp = interactionIcon.color;
        temp.a = 255;
        interactionIcon.color = temp;
        interactionIcon.sprite = iconSprite;
    }

    private void KillIcon()
    {
        Color temp = interactionIcon.color;
        temp.a = 0f;
        interactionIcon.color = temp;
        interactionIcon.sprite = null;
    }

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
        if (TriggeredInteraction!=null)
        {
            TriggeredInteraction();
        }
    }
    
    

}