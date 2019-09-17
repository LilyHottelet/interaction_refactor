using UnityEngine;

public class UIManager : MonoBehaviourSingleton<UIManager>
{
    public Sprite cursorLook;
    public Sprite cursorUse;
    public Sprite cursorTalk;


    private CursorUIElement currentCursor;
    public CursorUIElement cursorPrefab;
    public Transform cursorContainer;

    public void InitiateCursor(Interactable interactable)
    {
        if (currentCursor != null) return;

        currentCursor = Instantiate(cursorPrefab, cursorContainer);
        switch (interactable.type)
        {
            case InteractableType.Look:
                currentCursor.Initialize(cursorLook);
                break;
            case InteractableType.Use:
                currentCursor.Initialize(cursorUse);
                break;
            case InteractableType.Talk:
                currentCursor.Initialize(cursorTalk);
                break;
        }
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
        foreach (Transform child in cursorContainer)
        {
            Destroy(child.gameObject);
        }
        currentCursor = null;
    }
}

