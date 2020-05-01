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
        movementInput.y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetAxis("Jump");
    }
}

// This controller takes inspiration from Dani Devy's and WiggleWizard's implementation ( WiggleWizard ported a near 1:1 replica of Quake's movement )
public class PlayerControllerScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform playerCameraTransform;
    [SerializeField] Vector3 cameraOffset = Vector3.zero;
    [Space(5)]

    [Header("Look & Movement")]
    public Vector3 playerVelocity = Vector3.zero;
    Vector3 moveDirection = Vector3.zero;

    [SerializeField] float movementSpeed = 7;
    [SerializeField] float groundAcceleration = 14;
    [SerializeField] float groundDeceleration = 10;

    [SerializeField] float friction = 6;
    float currentFriction = 0;

    [SerializeField] float gravityStrength = 20;
    [SerializeField] float airControlPrecision = 0.3f;
    

    float viewPitch = 0;
    [Space(5)]
    [SerializeField] float mouseSensitivity = 10.0f;
    [SerializeField] float pitchLowerClamp = -90.0f;
    [SerializeField] float pitchUpperClamp = 90.0f;
    [Space(5)]

    [SerializeField] float jumpForce = 8;
    [Space(5)]

    // Name sounds weird, but the body the player has changes
    Rigidbody bodyRigidBody;

    CharacterController charController;

    InputManager inputManager;

    private void Awake()
    {
        bodyRigidBody = GetComponent<Rigidbody>();
        charController = GetComponent<CharacterController>();
        inputManager = new InputManager();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
   
    void Update()
    {
        inputManager.Update();
        MouseLook();
        Movement();
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

        playerCameraTransform.rotation = Quaternion.Euler(viewPitch, targetYaw, 0);
        transform.rotation = Quaternion.Euler(0, targetYaw, 0);

    }

    void ApplyFriction(float frictionModifier)
    {
        Vector3 playerVelCopy = playerVelocity;
        float speed, newSpeed;
        float drop = 0;
        float control = 0;

        playerVelCopy.y = 0;
        speed = playerVelCopy.magnitude;

        if (charController.isGrounded)
        {
            control = speed < groundDeceleration ? groundDeceleration : speed;
            drop = control * friction * Time.deltaTime * frictionModifier;
        }

        newSpeed = speed - drop;
        currentFriction = newSpeed;
        if (newSpeed < 0)
            newSpeed = 0;
        
        if(speed > 0)
            newSpeed /= speed;

        playerVelocity.x *= newSpeed;
        playerVelocity.z *= newSpeed;
    }

    void Accelerate(Vector3 desiredDirection, float desiredSpeed, float acceleration)
    {
        float addSpeed;
        float accSpeed;
        float curSpeed;

        curSpeed = Vector3.Dot(playerVelocity, desiredDirection);
        addSpeed = desiredSpeed - curSpeed;
        if (addSpeed <= 0)
            return;

        accSpeed = acceleration * Time.deltaTime * desiredSpeed;

        if (accSpeed > addSpeed)
            accSpeed = addSpeed;

        playerVelocity.x += accSpeed * desiredDirection.x;
        playerVelocity.z += accSpeed * desiredDirection.z;
    }

    void GroundMovement()
    {
        Vector3 desiredDirection;

        ApplyFriction(1.0f);

        desiredDirection = new Vector3(inputManager.movementInput.x, 0, inputManager.movementInput.y);
        // This transforms the input direction from the local space it is in to the world space relative to the gameobject's transform, then normalizes it
        desiredDirection = transform.TransformDirection(desiredDirection).normalized;

        moveDirection = desiredDirection;

        float desiredSpeed = desiredDirection.magnitude;
        desiredSpeed *= movementSpeed;

        Accelerate(desiredDirection, desiredSpeed, groundAcceleration);

        // ?Explore why this is necessary
        playerVelocity.y = -gravityStrength * Time.deltaTime;
    }

    void AirMovement()
    {
        playerVelocity.y -= gravityStrength * Time.deltaTime;
    }

    void Movement()
    {
        if (charController.isGrounded)
            GroundMovement();
        else
            AirMovement();

        charController.Move(playerVelocity * Time.deltaTime);
        playerCameraTransform.position = transform.position + cameraOffset;
    }
}
