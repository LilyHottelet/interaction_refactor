using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class InteractionHandler : MonoBehaviour
{
    [Header("Raycasting")]
    public LayerMask interactionLayerMask;
    private Ray ray;
    private RaycastHit hit;
    private Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
    private Camera mainCam;


    private Interactable currentInteractable;

    private void Start()
    {
        mainCam = CameraManager.Instance.mainCamera;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (GameStateManager.Instance.currentGameState != GameState.Default) return;

        if (IsHittingObject())
        {

            CheckForDifferentObjectHit();
            UIManager.Instance.DisplayInteractCursor(currentInteractable);
            ManageInput();

        }
        else
        {
            UIManager.Instance.KillInteractCursor();
        }
    }

    private void CheckForDifferentObjectHit()
    {
        Interactable hitObject = hit.collider.GetComponent<Interactable>();
        if (currentInteractable != hitObject)
        {
            currentInteractable = hitObject;
            UIManager.Instance.KillInteractCursor();
        }
    }

    private void ManageInput()
    {
        //This is fine for the prototype stage.
        //Would move to an input system like Rewired for production code.
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            currentInteractable.PlayInteraction();
        }
    }

    private bool IsHittingObject()
    {
        ray = mainCam.ScreenPointToRay(screenCenterPoint);
        return Physics.Raycast(ray, out hit, 1.8f, interactionLayerMask) && hit.transform.gameObject.CompareTag("Interactable");
    }

    [Conditional("UNITY_EDITOR")]
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(ray);
    }

}