using UnityEngine;

public class UIManager : MonoBehaviourSingleton<UIManager>
{

    [Header("Cursor Display")]
    public Sprite cursorLook;
    public Sprite cursorUse;
    public Sprite cursorTalk;
    public CursorUIElement cursorPrefab;
    public Transform cursorContainer;

    [Header("Game Screens")]
    public CloseInspectionScreen closeInspectionScreen;

    private CursorUIElement currentCursor;
    

    public void DisplayInteractCursor(Interactable interactable)
    {
        if (currentCursor != null) return;

        Vector3 cursorPosition = CameraManager.Instance.mainCamera.WorldToScreenPoint(interactable.cursorPosition.position);
        currentCursor = Instantiate(cursorPrefab, cursorPosition,Quaternion.identity,cursorContainer);

        switch (interactable.type)
        {
            case InteractableType.Look:
                currentCursor.Initialize(cursorLook, interactable.cursorPosition) ;
                break;
            case InteractableType.Use:
                currentCursor.Initialize(cursorUse, interactable.cursorPosition);
                break;
            case InteractableType.Talk:
                currentCursor.Initialize(cursorTalk, interactable.cursorPosition);
                break;
        }
    }

    public void KillInteractCursor()
    {
        foreach (Transform child in cursorContainer)
        {
            Destroy(child.gameObject);
        }
        currentCursor = null;
    }

    public void DisplayCloseInspectionScreen(Interactable interactable)
    {
        GameStateManager.Instance.SetGamePlaying(false);
        KillInteractCursor();
        closeInspectionScreen.gameObject.SetActive(true);
        closeInspectionScreen.Initialize(interactable);
    }
}

