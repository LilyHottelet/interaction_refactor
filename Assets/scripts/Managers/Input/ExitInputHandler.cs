using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitInputHandler : ManagerEvents
{

    [HideInInspector]
    public Props currentTarget;
    public GameObject currentTargetGo;
    public NPCs currentNpc;
    private Fading fading;

    private void Awake()
    {
        fading = GameObject.Find("fade").GetComponent<Fading>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (currentTarget!=null)
            {
                switch (currentTarget.type)
                {
                    case Props.TypeOfProp.Elevator:
                        OnFinishedPropInteraction(currentTargetGo,currentTarget);
                        break;
                    case Props.TypeOfProp.Look:
                        if (currentTarget.isNestedAction && !currentTarget.hasNextAction)
                        {
                            OnFinishedPropInteraction(currentTargetGo,currentTarget);
                            SetTarget();
                            return;
                        }
                        OnFinishedPropInteraction(currentTargetGo,currentTarget);
                        break;
                    case Props.TypeOfProp.Inspect:
                    case Props.TypeOfProp.Read:
                    case Props.TypeOfProp.Kinetoscope:
                                    
                        if (currentTarget.isNestedAction && !currentTarget.hasNextAction)
                        {
                            StartCoroutine(FadingProp(true));
                            return;
                        }
                        StartCoroutine(FadingProp(false));
                        break;
                }
                if (!currentTarget.isNestedAction)
                {
                    this.enabled = false;
                }
                
            }
            else if (currentNpc != null)
            {
                //StartCoroutine(FadingNpc());
                Debug.Log("hello");
            }
            else
            {
                Debug.Log("ExitInputHandler did not receive a target or npc");
            }
        }
        
    }
    
    private IEnumerator FadingProp(bool hasToSetTarget)
    {
        fading.StartFadeOut();
        yield return new WaitForSeconds(0.5f);
        OnFinishedPropInteraction(currentTargetGo,currentTarget);
        if (hasToSetTarget)
        {
            SetTarget();
        }
    }

    private void SetTarget()
    {
        currentTargetGo = currentTargetGo.transform.parent.gameObject;
        currentTarget = currentTargetGo.GetComponent<PropComponent>().prop;
    }

}