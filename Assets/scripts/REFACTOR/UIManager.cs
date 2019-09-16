using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviourSingleton<UIManager>
{

    public void InitiateCursor(Interactable interactable)
    {
        //interactionIcon.transform.position = raycastCam.WorldToScreenPoint(cursorPosition.position);
        //exitInteractionNotified = false;
        //if (!interactionNotified)
        //{
        //    interactionNotified = true;
        //    UIManager.Instance.DisplayIcon(cursorSprite);
        //}
    }

    //Change this to instantiating an icon at the correct place
    public void DisplayIcon(Sprite iconSprite)
    {
        //Color temp = interactionIcon.color;
        //temp.a = 255;
        //interactionIcon.color = temp;
        //interactionIcon.sprite = iconSprite;
    }

    //also change it to kill the prefab
    public void KillIcon()
    {
        //Color temp = interactionIcon.color;
        //temp.a = 0f;
        //interactionIcon.color = temp;
        //interactionIcon.sprite = null;
    }
}

