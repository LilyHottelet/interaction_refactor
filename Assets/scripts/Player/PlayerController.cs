using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour
{
    public float walkingSpeed = 2f, runningSpeed = 4f;
    private float currentSpeed;
    public float sensitivityH = 2f;
    public float sensitivityV = 5f;
    public float maxClampV = 60f;
    public float minClampV = -60f;

    public Transform eyes;
    public bool isWalking = false, isRunning = false;
    private Animator animator;
    public bool isAbleToWalk = true;
    private float previousMousePosX;
    private float previousMousePosY;
    public float smoothTime = 5f;
    //temp test
    public Transform lobbyPlayer;
    private Quaternion characterTargetRot;
    private Quaternion cameraTargetRot;

    private void Awake()
    {
        ManagerEvents.LevelContextUpdated += OnLevelContextUpdate;
        ManagerEvents.ChangedRoom += OnChangeRoom;
        ManagerEvents.MustLoadScene += OnMustLoad;
        
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        eyes.transform.Rotate(0, 0, 0);
        Cursor.lockState = CursorLockMode.Locked;

        cameraTargetRot = eyes.transform.localRotation;
        characterTargetRot = transform.localRotation;
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal") * currentSpeed * Time.deltaTime;
        float moveVertical = Input.GetAxis("Vertical") * currentSpeed * Time.deltaTime;
        
        if (isAbleToWalk && Input.GetAxis("Vertical") != 0 || isAbleToWalk && Input.GetAxis("Horizontal") != 0) 
        {
            transform.Translate(moveHorizontal, 0, moveVertical);
            isWalking = true;
            animator.SetBool("walking", true);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                isRunning = true;
                animator.SetBool("running", true);
                currentSpeed = runningSpeed;
            }
            else
            {
                isRunning = false;
                animator.SetBool("running", false);
                currentSpeed = walkingSpeed;
            }
        }
        else
        {
            isWalking = false;
            animator.SetBool("walking", false);
        }

        RotateCamera();

    }

    private void RotateCamera()
    {
        /*if (Input.GetAxis("Mouse X")!= previousMousePosX || Input.GetAxis("Mouse Y") != previousMousePosY)
        {*/

        cameraTargetRot = eyes.transform.localRotation;
        characterTargetRot = transform.localRotation;
        float yRot = Input.GetAxis("Mouse X") * sensitivityH;
        float xRot = Input.GetAxis("Mouse Y") * sensitivityV;

        characterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
        cameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

        cameraTargetRot = ClampRotationAroundXAxis(cameraTargetRot);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, characterTargetRot,
            smoothTime * Time.deltaTime);
        eyes.transform.localRotation = Quaternion.Slerp(eyes.transform.localRotation, cameraTargetRot,
            smoothTime * Time.deltaTime);
//        previousMousePosX = Input.GetAxis("Mouse X");
//        previousMousePosY = Input.GetAxis("Mouse Y");
    //}

}
    
    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

        angleX = Mathf.Clamp (angleX, minClampV, maxClampV);

        q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
    
    public void SetPlayerPosition(float[] position,float[] rotation)
    {
        Vector3 tempPos = transform.position;
        tempPos.x = position[0];
        tempPos.y = position[1];
        tempPos.z = position[2];
        Vector3 tempRot = transform.eulerAngles;
        tempRot.x = rotation[0];
        tempRot.y = rotation[1];
        tempRot.z = rotation[2];
        transform.position = tempPos;
        transform.eulerAngles = tempRot;
    }
    
    private void OnLevelContextUpdate(PlayerData data)
    {
        if (data!=null)
        {
            SetPlayerPosition(data.playerPosition,data.playerRotation);
        }
    }

    private void OnChangeRoom(Door door)
    {
        Debug.Log("testPLacement");
        float[] positionCoordinates = new float[3];
        float[] rotationCoordinates = new float[3];
        positionCoordinates[0] = door.spawnPoint.position.x;
        positionCoordinates[1] = door.spawnPoint.position.y;
        positionCoordinates[2] = door.spawnPoint.position.z;
        rotationCoordinates[0] = door.spawnPoint.rotation.eulerAngles.x;
        rotationCoordinates[1] = door.spawnPoint.rotation.eulerAngles.y;
        rotationCoordinates[2] = door.spawnPoint.rotation.eulerAngles.z;
        SetPlayerPosition(positionCoordinates,rotationCoordinates);
        
    }

    private void OnMustLoad(Room roomToLoad)
    {
        float[] positionCoordinates = new float[3];
        float[] rotationCoordinates = new float[3];
        positionCoordinates[0] = roomToLoad.spawnPoint.position.x;
        positionCoordinates[1] = roomToLoad.spawnPoint.position.y;
        positionCoordinates[2] = roomToLoad.spawnPoint.position.z;
        rotationCoordinates[0] = roomToLoad.spawnPoint.rotation.eulerAngles.x;
        rotationCoordinates[1] = roomToLoad.spawnPoint.rotation.eulerAngles.y;
        rotationCoordinates[2] = roomToLoad.spawnPoint.rotation.eulerAngles.z;
        StartCoroutine(PlacePlayer(positionCoordinates,rotationCoordinates));
    }

   

   

    public void ResetCamRotation()
    {
        eyes.transform.localRotation = Quaternion.identity;
    }
    
    private IEnumerator PlacePlayer( float[] coordinatesPos,float[]coordinatesRot)
    {
        yield return new WaitForSeconds(1f);
        SetPlayerPosition(coordinatesPos,coordinatesRot);
        Debug.Log("placing player");
    }
}