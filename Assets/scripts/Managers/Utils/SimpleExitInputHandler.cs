using UnityEngine;

public class SimpleExitInputHandler : MonoBehaviour {

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			gameObject.SetActive(false);
		}
	}
}
