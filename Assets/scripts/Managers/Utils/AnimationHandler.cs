using System.Collections;
using UnityEngine;

public class AnimationHandler : ManagerEvents
{

    private Animator animator;
    
    private void OnEnable()
    {
        ManagerEvents.StartedPropInteraction += OnStartedPropInteracting;
        ManagerEvents.FinishedPropInteraction += OnFinishedPropInteracting;
    }
    
    private void OnDisable()
    {
        ManagerEvents.StartedPropInteraction -= OnStartedPropInteracting;
        ManagerEvents.FinishedPropInteraction -= OnFinishedPropInteracting;
    }

    private void OnStartedPropInteracting(GameObject go, Props prop)
    {
        if (prop.hasAnimation)
        {
            animator = go.GetComponent<Animator>();
            if (!prop.animationTriggered)
            {
                animator.SetTrigger("playAnimation");
                if (!prop.isDrawer)
                {
                    prop.animationTriggered = true;
                }
            }
            
            
            StartCoroutine(SetAnimator(false,GetAnimationLength()));
        }
    }

    private void OnFinishedPropInteracting(GameObject go,Props prop)
    {
        if (prop.hasAnimation)
        {            
            StartCoroutine(SetAnimator(true,0));
            if (prop.isDrawer)
            {
                animator.SetTrigger("rewindAnimation");
            }
           
        }
    }

    private IEnumerator SetAnimator(bool isActive,float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.enabled = isActive;
    }

    private float GetAnimationLength()
    {
        float length = animator.runtimeAnimatorController.animationClips[0].length;
        string name = animator.runtimeAnimatorController.animationClips[0].name;
        Debug.Log("The "+name+" animation has a length of "+length);
        return length;
    }
}