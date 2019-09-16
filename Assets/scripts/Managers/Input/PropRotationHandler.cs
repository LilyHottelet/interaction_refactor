using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRotationHandler : ManagerEvents {

	public float speedX;
	public float speedY;
	[HideInInspector]
	public Transform propToRotate;
	public GameObject parentRotate;
	public GameObject currentTargetGo;
	public Texture2D rotationCursor;
	public Texture2D test;


	private void OnEnable()
	{
		
		Invoke("DisplayRotationCursor", 1f);
	}

	private void OnDisable()
	{
		KillRotationCursor();
	}


	void FixedUpdate()
	{

		if (Input.GetMouseButton (0))
		{
			//propToRotate =  parentRotate.transform.GetChild(0);
			Cursor.SetCursor(test,Vector2.zero,CursorMode.Auto);
			propToRotate = currentTargetGo.transform;
			float rotx = Input.GetAxis ("Mouse X") * Time.deltaTime * speedX * Mathf.Deg2Rad;
			float roty = Input.GetAxis ("Mouse Y") * Time.deltaTime * speedY * Mathf.Deg2Rad;
			propToRotate.RotateAround (Vector2.right, roty);
			propToRotate.RotateAround (Vector2.up, -rotx);
		}
		else
		{
			Cursor.SetCursor(rotationCursor,Vector2.zero,CursorMode.Auto);
		}
	}

	private void DisplayRotationCursor()
	{
		Cursor.SetCursor(rotationCursor,Vector2.zero,CursorMode.Auto);
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	private void KillRotationCursor()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		
	}
	

}
