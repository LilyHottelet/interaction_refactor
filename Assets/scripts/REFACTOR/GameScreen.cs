using UnityEngine;

public abstract class GameScreen : MonoBehaviour
{
    public Texture2D cursorToDisplay;

    protected virtual void OnEnable()
    {
        //pause game
        DisplayCustomCursor();
    }

    protected virtual void OnDisable()
    {
        RevertCursor();
    }

    private void DisplayCustomCursor()
    {
        if (cursorToDisplay)
        {
            Cursor.SetCursor(cursorToDisplay, Vector2.zero, CursorMode.Auto);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void RevertCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public abstract void Initialize(Interactable interactable);
}
