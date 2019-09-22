using UnityEngine;

public class CloseInspectionScreen : GameScreen {

	public float speedX;
	public float speedY;

    private GameObject currentTargetGo;
	public Texture2D clickingCursor;

	public override void Initialize(Interactable interactable)
	{
        currentTargetGo = interactable.gameObject;
	}

	void FixedUpdate()
	{

		if (Input.GetMouseButton (0))
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
