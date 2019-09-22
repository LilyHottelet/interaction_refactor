using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableProp : Interactable
{
    public override void PlayInteraction()
    {
        UIManager.Instance.DisplayCloseInspectionScreen(this);
    }
    
}
