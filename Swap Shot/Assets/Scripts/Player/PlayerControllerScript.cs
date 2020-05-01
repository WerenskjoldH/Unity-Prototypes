using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager
{
    public Vector2 movementInput;
    public Vector2 mouseInput;
    public float jumping = 0;

    public void Update()
    {
        mouseInput.x = Input.GetAxis("Mouse X");
        mouseInput.y = Input.GetAxis("Mouse Y");
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxis("Vertical");
        jumping = Input.GetAxis("Jump");
    }
}

// This controller takes inspiration from Dani Devy's implementation
public class PlayerControllerScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform worldOrientationTransform;
    [SerializeField] Transform playerCameraTransform;
    [Space(5)]

    [Header("Look & Movement")]
    [SerializeField] float movementSpeed = 500;
    [SerializeField] float maxSpeed = 20;
    

    float viewPitch = 0;
    [Space(5)]
    [SerializeField] float mouseSensitivity = 10.0f;
    [SerializeField] float pitchLowerClamp = -90.0f;
    [SerializeField] float pitchUpperClamp = 90.0f;
    [Space(5)]

    [SerializeField] float jumpForce = 500;
    // This is in degrees
    [SerializeField] float maxSlope = 50.0f;
    [Space(5)]

    [Header("Properties")]
    public bool isGrounded = false;
    public float groundRaycastLength = 1f;
    // Name sounds weird, but the body the player has changes
    Rigidbody bodyRigidBody;

    InputManager inputManager;

    private void Awake()
    {
        bodyRigidBody = GetComponent<Rigidbody>();
        inputManager = new InputManager();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    // Movement is done through forces, so best lock it into a fixed update
    private void FixedUpdate()
    {
        Movement();
    }

    void Update()
    {
        inputManager.Update();
        MouseLook();
    }

    // This should be moved to using a spherecast
    private void CheckGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, groundRaycastLength))
        {
            isGrounded = true;
        }
        else
            isGrounded = false;
        Debug.DrawRay(transform.position, new Vector3(0, -groundRaycastLength, 0), Color.white);
    }

    void MouseLook()
    {
        float mouseX = inputManager.mouseInput.x * mouseSensitivity * Time.fixedDeltaTime;
        float mouseY = inputManager.mouseInput.y * mouseSensitivity * Time.fixedDeltaTime;

        Vector3 currentRotation = playerCameraTransform.localEulerAngles;

        float targetYaw = currentRotation.y + mouseX;

        viewPitch -= mouseY;
        // We shouldn't be able to do front/back tucks, so lock that pitch between target values;
        viewPitch = Mathf.Clamp(viewPitch, pitchLowerClamp, pitchUpperClamp);

        playerCameraTransform.localRotation = Quaternion.Euler(viewPitch, targetYaw, 0);
        worldOrientationTransform.localRotation = Quaternion.Euler(0, targetYaw, 0);
    }

    void Movement()
    {
        float velocityModifier = 1.0f;

        CheckGrounded();
        if (inputManager.jumping == 1 && isGrounded)
            bodyRigidBody.AddForce(worldOrientationTransform.up * jumpForce);

        if (!isGrounded)
            velocityModifier *= 0.5f;

        bodyRigidBody.AddForce(inputManager.movementInput.x * worldOrientationTransform.right * movementSpeed * velocityModifier * Time.fixedDeltaTime);
        bodyRigidBody.AddForce(inputManager.movementInput.y * worldOrientationTransform.forward * movementSpeed * velocityModifier * Time.fixedDeltaTime);
    }
}
