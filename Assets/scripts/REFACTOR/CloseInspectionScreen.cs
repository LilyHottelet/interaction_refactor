using UnityEngine;
using UnityEngine.UI;

public class CloseInspectionScreen : GameScreen {

    [Header("Rotation Settings")]
    public float speedX;
	public float speedY;
    public Text debugText;

    [Space(10)]
    private GameObject currentTargetGo;
	public Texture2D clickingCursor;
    private bool initialized = false;

	public override void Initialize(Interactable interactable)
	{
        currentTargetGo = interactable.gameObject;
        debugText.text = interactable.name;
        initialized = true;
    }

    void FixedUpdate()
	{
        if (!initialized) return;

        if (Input.GetMouseButton(0))
		{
			Cursor.SetCursor(clickingCursor, Vector2.zero,CursorMode.Auto);
			float rotx = Input.GetAxis ("Mouse X") * Time.deltaTime * speedX * Mathf.Deg2Rad;
			float roty = Input.GetAxis ("Mouse Y") * Time.deltaTime * speedY * Mathf.Deg2Rad;
            currentTargetGo.transform.RotateAround (Vector2.right, roty);
            currentTargetGo.transform.RotateAround (Vector2.up, -rotx);
		}
		else
		{
			Cursor.SetCursor(cursorToDisplay,Vector2.zero,CursorMode.Auto);
		}
	}
}
