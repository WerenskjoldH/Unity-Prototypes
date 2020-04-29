using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This controller takes inspiration from Dani Devy's implementation

public class PlayerControllerScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform worldOrientationTransform;
    [SerializeField] Transform playerCameraTransform;
    [Space(5)]

    [Header("Look & Movement")]
    [SerializeField] float movementSpeed = 2000;
    [SerializeField] float maxSpeed = 20;
    

    float viewPitch = 0;
    [Space(5)]
    [SerializeField] float mouseSensitivity = 10.0f;
    [SerializeField] float pitchLowerClamp = -90.0f;
    [SerializeField] float pitchUpperClamp = 90.0f;
    [Space(5)]

    [SerializeField] float jumpForce = 500;
    [SerializeField] float maxSlope = 50.0f;
    [Space(5)]

    [SerializeField] float frictionForce = 0.15f;
    [Space(5)]

    [Header("Properties")]
    public bool isGrounded = false;
    // Name sounds weird, but we can change bodies
    Rigidbody bodyRigidBody;

    private void Awake()
    {
        bodyRigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    // Movement is done through forces, so best lock it into a fixed update
    private void FixedUpdate()
    {
        
    }

    void Update()
    {
        MouseLook();
    }

    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.fixedDeltaTime;

        Vector3 currentRotation = playerCameraTransform.localEulerAngles;

        float targetYaw = currentRotation.y + mouseX;

        viewPitch -= mouseY;
        // We shouldn't be able to do front/back tucks, so lock that pitch between target values;
        viewPitch = Mathf.Clamp(viewPitch, pitchLowerClamp, pitchUpperClamp);

        playerCameraTransform.localRotation = Quaternion.Euler(viewPitch, targetYaw, 0);
        worldOrientationTransform.localRotation = Quaternion.Euler(0, targetYaw, 0);
    }
}
