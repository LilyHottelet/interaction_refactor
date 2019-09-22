using System.Diagnostics;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    [Header("Raycasting")]
    public Camera mainCam;
    public LayerMask interactionLayerMask;
    private Ray ray;
    private RaycastHit hit;
    private Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);

    private Interactable currentInteractable;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Cast a ray to the center of the screen
        if (IsHittingObject())
        {
            currentInteractable = hit.collider.GetComponent<Interactable>();
            UIManager.Instance.DisplayInteractCursor(currentInteractable);
            ManageInput();
        }
        else
        {
            UIManager.Instance.KillInteractCursor();
        }
    }

    private void ManageInput()
    {
        //This is fine for the prototype stage.
        //Would move to an input system like Rewired for production code.
        if (Input.GetKeyDown(KeyCode.Mouse0) && GameManager.canInteract)
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