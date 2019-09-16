using UnityEngine;
   
public class closeInteraction : MonoBehaviour
{
   
    private Camera interactCam;
    private float previousMousePosX, previousMousePosY;
    private float sensitivity = 6f;
    private Quaternion cameraTargetRot;
    private Quaternion characterTargetRot;
    private Transform parentRotate;
    private InteractionDetector interactionDetector;
   
    private void Awake()
    {
        interactCam = transform.GetComponent<Camera>();
        parentRotate = gameObject.transform;
        
    }

    private void OnEnable()
    {
        characterTargetRot = parentRotate.transform.localRotation;
        cameraTargetRot = interactCam.transform.localRotation;
    }
    private void Update()
    {
        //@TODO CLAMP

        float yRot = Input.GetAxis("Mouse X") * sensitivity;
        float xRot = Input.GetAxis("Mouse Y") * sensitivity;

        cameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);
        characterTargetRot *= Quaternion.Euler(0f, yRot, 0f);

        interactCam.transform.localRotation =
            Quaternion.Slerp(interactCam.transform.localRotation, cameraTargetRot, 5f * Time.deltaTime);
        parentRotate.localRotation =
            Quaternion.Slerp(parentRotate.localRotation, characterTargetRot, 5f * Time.deltaTime);
    }
}