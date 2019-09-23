using System.Collections;
using UnityEngine;


public class CameraManagerOld : ManagerEvents
{

    public Camera mainCam, propsCam;
    public float nearView, farView;
    public GameObject UIprops,UImain;
    
    private GamePausing gamePausing;
    private InteractionDetector interactionDetector;
    private Props currentTarget;
    private GameObject currentTargetGo;
    private GameObject currentCloseCam;
    private Transform parentTarget;
    public float distance = 0.3f;
    private Vector3 propOriginPos;
    private Quaternion propOriginRot;
    private Transform playerPos;

    [SerializeField]
    private Transform originPlayerTransform;
    [SerializeField]
    private Transform originCamTransform;

    public float lerpSpeedValue = 1f;

    private void Awake()
    {
        gamePausing = GameObject.Find("Managers").GetComponent<GamePausing>();
        interactionDetector = GameObject.Find("Input").GetComponent<InteractionDetector>();
        
        propsCam.enabled = false;
        UIprops.SetActive (false);
        UImain.SetActive (true);
    }

    private void OnEnable()
    {
        ManagerEvents.StartedPropInteraction += OnStartedPropInteracting;
        ManagerEvents.StartedNpcInteraction += OnStartedNpcInteracting;
        ManagerEvents.FinishedPropInteraction += OnFinishedPropInteracting;
        ManagerEvents.FinishedNpcInteraction += OnFinishedNpcInteracting;
    }

    private void OnDisable()
    {
        ManagerEvents.StartedPropInteraction -= OnStartedPropInteracting;
        ManagerEvents.StartedNpcInteraction -= OnStartedNpcInteracting;
        ManagerEvents.FinishedPropInteraction -= OnFinishedPropInteracting;
        ManagerEvents.FinishedNpcInteraction -= OnFinishedNpcInteracting;
    }

    private void OnStartedPropInteracting(GameObject go,Props prop)
    {
        currentTarget = prop;
        currentTargetGo = go;
        switch (prop.type)
        {
            case Props.TypeOfProp.Look:
                if (prop.isNestedAction)
                {
                    SwitchToMainCam();
                }
                currentCloseCam = currentTargetGo.GetComponent<PropComponent>().propCamera;
                StartCoroutine(LerpToPosition(currentCloseCam.transform, lerpSpeedValue, true));
                break;
            case Props.TypeOfProp.Read:
                if (prop.isNestedAction)
                {
                    SwitchToMainCam();
                }
                Invoke("TriggerRemark",1f);
                break;
            case Props.TypeOfProp.Inspect:
                if (prop.isNestedAction)
                {
                    SwitchToMainCam();
                }
                else
                {
                    ResetCamRotation();
                }
                MoveObject(go);
                ChangeToPropView();
                Invoke("TriggerRemark",1f);
                break;
            case Props.TypeOfProp.Kinetoscope:
                SetKinetoscopeCam(go,true);
                break;
        }
    }

    private void OnStartedNpcInteracting(NPCs npc,GameObject go,string passage)
    {
        ChangeToCharCam(go.GetComponent<NPCComponent>().npcCam);
    }

    private void OnFinishedPropInteracting(GameObject go,Props prop)
    {
        currentTarget = prop;
        switch (prop.type)
        {
            case Props.TypeOfProp.Inspect:
                ResetProp(go);
                if (prop.isNestedAction)
                {
                    Transform parentPositioning = GetParentPosition(go);
                    LookAtTarget(parentPositioning); 
                    SwitchToCloseCam();
                }
                ChangeToMainView();
                
                break;
            case Props.TypeOfProp.Look:
                if (prop.isNestedAction)
                {
                    Transform parentPositioning = GetParentPosition(go);
                    StartCoroutine(LerpToPosition(parentPositioning,lerpSpeedValue, false));
                }
                else
                {
                    SwitchToMainCam();                 
                    StartCoroutine(LerpToPosition(originCamTransform,lerpSpeedValue, false));
                }
                break;
            case Props.TypeOfProp.Read:
                if (prop.isNestedAction)
                {
                    Transform parentPositioning = GetParentPosition(go);
                    LookAtTarget(parentPositioning);
                    SwitchToCloseCam();
                }
                break;
            case Props.TypeOfProp.Kinetoscope:
                SetKinetoscopeCam(go,false);
                break;
        }
    }

    private void OnFinishedNpcInteracting(NPCs npc,GameObject go)
    {
        go.GetComponent<NPCComponent>().npcCam.enabled = false;
        ChangeToMainView();
    }

    public void ChangeToPropView()
    {
        propsCam.enabled = true;
        mainCam.enabled = true;
        mainCam.GetComponent<Camera>().fieldOfView = nearView;
    }

    //@TODO improve this method
    public void ChangeToMainView()
    {
        mainCam.GetComponent<Camera>().fieldOfView = farView;
        mainCam.enabled = true;
    }

    public void ChangeToCharCam(Camera npcCam)
    {
        npcCam.enabled = true;

    }

    public void ResetCamRotation()
    {
        mainCam.transform.localRotation = Quaternion.identity;
    }

    private void LookAtTarget(Transform target)
    {
        mainCam.transform.position = target.position;
        mainCam.transform.rotation = target.rotation;
    }
    
    IEnumerator LerpToPosition(Transform destinationCamera, float lerpSpeed, bool isForward)
    {

        float t = 0.0f;
        //Positions
        Vector3 startingCamPos = mainCam.transform.position;
        Vector3 endCamPos = destinationCamera.position;
        //Rotations
        Quaternion startingCamRot = mainCam.transform.rotation;
        Quaternion endCamRot = destinationCamera.rotation;
        
        
        //First level of Interaction
        if (!currentTarget.isNestedAction && isForward)
        {
            originCamTransform.position = mainCam.transform.position;
            originCamTransform.rotation = mainCam.transform.rotation;

        } 
        
        //Movement over time
        while (t < 1f)
        {
            t += Time.deltaTime * (Time.timeScale / lerpSpeed);
            //Moving the camera
            mainCam.transform.position = Vector3.Lerp(startingCamPos, endCamPos, t);
            mainCam.transform.rotation = Quaternion.Lerp(startingCamRot, endCamRot, t);
            yield return 0;
        }
        //Intermediate Level of Interaction
        if ((isForward && currentTarget.canInteractClose) || (!isForward && currentTarget.isNestedAction))
        {
            gamePausing.EnableCloseInteraction();
            if (isForward && currentTarget.canInteractClose)
            {
                SwitchToCloseCam();
            }
            
        }
        //Back to original Position
        if (!isForward && !currentTarget.isNestedAction)
        {
            gamePausing.PlayGame();
        }
        
        if (isForward)
        {
            OnDetectiveRemarkStarted(currentTarget.clue);  
        }
    }
    
    private void MoveObject(GameObject go)
    {
        propOriginPos = go.transform.position;
        propOriginRot = go.transform.rotation;
        //go.transform.rotation = Quaternion.identity;
        go.transform.position = mainCam.transform.position + mainCam.transform.forward * distance;
        go.layer = 8;
    }

    private void ResetProp(GameObject go)
    {
        go.transform.rotation = propOriginRot;
        go.transform.position = propOriginPos;
        go.layer = 0;
    }

    private Transform GetParentPosition(GameObject go)
    {
        parentTarget = go.transform.parent;
        return parentTarget.transform.GetComponent<PropComponent>().propCamera.transform;
    }

    private void TriggerRemark()
    {
        OnDetectiveRemarkStarted(currentTarget.clue);
    }

    private void SwitchToCloseCam()
    {
        interactionDetector.raycastCam = currentCloseCam.GetComponent<Camera>();
        currentCloseCam.SetActive(true);
    }

    private void SwitchToMainCam()
    {
        interactionDetector.raycastCam = mainCam;
        currentCloseCam.SetActive(false);
    }

    private void SetKinetoscopeCam(GameObject go, bool isEnabling)
    {
        
        GameObject kinCam = go.GetComponent<PropComponent>().propCamera;
        kinCam.SetActive(isEnabling);

    }

  
}