using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampedRotation : MonoBehaviour {

    private float previousMousePosX;
    private float previousMousePosY;
    public float sensitivityH = 2f;
    public float sensitivityV = 5f;
    public float smoothTime = 5f;
    public float minClamp = -20f;
    public float maxClamp = 20f;
	
    private Quaternion characterTargetRot;
    private Quaternion cameraTargetRot;
    private Quaternion clampTargetRot;

    
    public Transform clampController;
    public Transform eyes;


    private void Start()
    {
        clampController = gameObject.transform.parent;
        eyes = gameObject.transform;
    }
    private void Update()
    {
        RotateCamera();
		
    }
	
    private void RotateCamera()
    {
        if (Input.GetAxis("Mouse X")!= previousMousePosX || Input.GetAxis("Mouse Y") != previousMousePosY)
        {
        
            cameraTargetRot = eyes.transform.localRotation;
            characterTargetRot = transform.localRotation;
            clampTargetRot = clampController.localRotation;
            float yRot = Input.GetAxis("Mouse X") * sensitivityH;
            float xRot = Input.GetAxis("Mouse Y") * sensitivityV;

            characterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
            clampTargetRot *= Quaternion.Euler (0f, yRot, 0f);
            cameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);
            
            cameraTargetRot = ClampRotationAroundXAxis (cameraTargetRot);
            clampTargetRot = ClampRotationAroundYAxis (clampTargetRot);
            
            clampController.localRotation = Quaternion.Slerp(clampController.localRotation, clampTargetRot, smoothTime * Time.deltaTime);
            eyes.transform.localRotation = Quaternion.Slerp (eyes.transform.localRotation, cameraTargetRot,
                smoothTime * Time.deltaTime);


            
            previousMousePosX = Input.GetAxis("Mouse X");
            previousMousePosY = Input.GetAxis("Mouse Y");
        }
        
    }
    
    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

        angleX = Mathf.Clamp (angleX, minClamp, maxClamp);

        q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
    
    Quaternion ClampRotationAroundYAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.y);

        angleY = Mathf.Clamp (angleY, minClamp, maxClamp);
        q.y = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleY);

        return q;
    }
}