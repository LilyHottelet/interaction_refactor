using UnityEngine;

public class ClampedRotationTest : MonoBehaviour
{

    private Quaternion characterTargetRot;
    private Quaternion cameraTargetRot;
    private float previousMousePosX;
    private float previousMousePosY;
    private float sensitivity = 10f;
    public Camera eyes;


    public Transform startTransform;
    private Quaternion startRotation;
    public float damping = 5f;

    private Quaternion yTargetRot;
    private Quaternion xTargetRot;
    

    private void Start()
    {
        cameraTargetRot = eyes.transform.localRotation;
        characterTargetRot = transform.localRotation;
        startRotation = startTransform.localRotation;

    }
    public float sensitivityX = 2F;
    public float minimumX = -30F;
    public float maximumX = 30F;

    float rotationX = 0f;

    private void FixedUpdate()
    {
        if (Input.GetAxis("Mouse X") > 0 || Input.GetAxis("Mouse Y") > 0)
        {
            Quaternion camRot =  transform.localRotation;
            rotationX += Input.GetAxis ("Mouse X") * sensitivityX;
            rotationX = Mathf.Clamp (rotationX, minimumX, maximumX);
            float angle=Mathf.Abs(Quaternion.Angle(camRot, startRotation));
            float ratio = Mathf.Max(maximumX-angle,0f)/maximumX;
            Debug.Log(ratio);
            rotationX *=ratio ;
//        rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
//        rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
            transform.localEulerAngles = new Vector3(0, rotationX, 0);
//            eyes.transform.localEulerAngles = new Vector3 (-rotationY, 0, 0);
        }
    }


    private void TestRotateClamp()
    {
        
        if (Input.GetAxis("Mouse X") > 0 || Input.GetAxis("Mouse Y") > 0)
        {   
            float yRot = Input.GetAxis("Mouse X") * sensitivity;
            characterTargetRot *= Quaternion.Euler (0f, yRot, 0f);	
            float angle=Mathf.Abs(Quaternion.Angle(transform.localRotation, startRotation));
            if(angle>7)return;

            transform.localRotation = Quaternion.Slerp (transform.localRotation, characterTargetRot,damping*Time.deltaTime);
            eyes.transform.localRotation = Quaternion.Slerp (eyes.transform.localRotation, cameraTargetRot,damping*Time.deltaTime); 
            
        }
    }  
}