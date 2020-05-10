﻿using UnityEngine;
using Cinemachine;

public class InputManager
{
    public Vector2 movementInput;
    public Vector2 mouseInput;
    public bool queuedJump = false;

    public void Update()
    {
        mouseInput.x = Input.GetAxis("Mouse X");
        mouseInput.y = Input.GetAxis("Mouse Y");
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump") && !queuedJump)
            queuedJump = true;
        if (Input.GetButtonUp("Jump"))
            queuedJump = false;
    }
}

// This controller takes inspiration from WiggleWizard's implementation ( WiggleWizard ported a near 1:1 replica of Quake's movement )
public class PlayerControllerScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CinemachineVirtualCamera playerCamera;
    [SerializeField] Vector3 cameraOffset = Vector3.zero;
    [Space(5)]

    [Header("Look & Movement")]
    Vector3 playerVelocity = Vector3.zero;
    // This is primarily for debugging
    Vector3 moveDirection = Vector3.zero;

    [SerializeField] float surfaceRaycastLength = 1.5f;
    bool playerGrounded = false;

    [SerializeField] float movementSpeed = 7;
    [SerializeField] float groundAcceleration = 14;
    [SerializeField] float groundDeceleration = 10;

    [SerializeField] float airControlPrecision = 0.3f;
    [SerializeField] float minimumSpeedForAirControl = 0.001f;
    [SerializeField] float airAcceleration = 2;
    [SerializeField] float airDeceleration = 2;
    [SerializeField] float sideStrafeSpeed = 1;
    [SerializeField] float sideStrafeAcceleration = 50;
    [SerializeField] float jumpSpeed = 8;

    [SerializeField] float slopeDescentMultiplier = 4.0f;

    [SerializeField] float friction = 6;
    float currentFriction = 0;

    [SerializeField] float gravityStrength = 20;    

    float viewPitch = 0;
    [Space(5)]
    [SerializeField] float mouseSensitivity = 10.0f;
    [SerializeField] float pitchLowerClamp = -90.0f;
    [SerializeField] float pitchUpperClamp = 90.0f;
    [Space(5)]

    [SerializeField] float jumpForce = 8;
    [Space(5)]

    [Header("Player Info")]
    bool isAlive = true;

    [Space(5)]

    [Header("Debug Options")]
    [SerializeField] bool debugVectors;

    RaycastHit groundHit;
    Quaternion fromUpToGroundNormal;
    Vector3 downSlopeVector;

    CharacterController charController;

    InputManager inputManager;

    #region Getters & Setters

    public bool GetPlayerAlive()
    {
        return isAlive;
    }

    public void SetPlayerAlive(bool t)
    {
        isAlive = t;
    }

    // The character controller overrides attempts to change position
    public void SetPosition(Vector3 p)
    {
        charController.enabled = false;
        transform.position = p;
        charController.enabled = true;
    }

    #endregion

    void Awake()
    {
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
    }

    void FixedUpdate()
    {

        CheckGroundedSurface();
        Movement();
    }

    #region Input & Movement

    void CheckGroundedSurface()
    {
        if (Physics.Raycast(transform.position, -1 * transform.up, out groundHit, surfaceRaycastLength))
        {
            fromUpToGroundNormal = Quaternion.FromToRotation(transform.up, groundHit.normal);
            Vector3 nCrossUp = Vector3.Cross(groundHit.normal, Vector3.up);
            downSlopeVector = Vector3.Cross(groundHit.normal, nCrossUp);
        }
        else
        {
            fromUpToGroundNormal = Quaternion.identity;
            playerGrounded = false;
        }
    }

    Vector3 CalculateRawDesiredDirection()
    {
        Vector3 desiredDirection = new Vector3(inputManager.movementInput.x, 0, inputManager.movementInput.y);
        desiredDirection = transform.TransformDirection(desiredDirection);
        return desiredDirection;
    }

    void MouseLook()
    {
        float mouseX = inputManager.mouseInput.x * mouseSensitivity * Time.fixedDeltaTime;
        float mouseY = inputManager.mouseInput.y * mouseSensitivity * Time.fixedDeltaTime;
        Vector3 currentRotation = playerCamera.transform.localEulerAngles;
        float targetYaw = currentRotation.y + mouseX;

        viewPitch -= mouseY;
        // We shouldn't be able to do front/back tucks, so lock that pitch between target values
        viewPitch = Mathf.Clamp(viewPitch, pitchLowerClamp, pitchUpperClamp);

        playerCamera.transform.rotation = Quaternion.Euler(viewPitch, targetYaw, 0);
        transform.rotation = Quaternion.Euler(0, targetYaw, 0);
    }

    void ApplyFriction(float frictionModifier)
    {
        Vector3 playerVelCopy = playerVelocity;
        float speed, newSpeed;
        float drop = 0;

        playerVelCopy.y = 0;
        speed = playerVelCopy.magnitude;

        if (playerGrounded)
        {
            float control = speed < groundDeceleration ? groundDeceleration : speed;
            drop = control * friction * Time.deltaTime * frictionModifier;
        }

        newSpeed = speed - drop;
        currentFriction = newSpeed;
        if (newSpeed < 0)
            newSpeed = 0;
        
        if(speed > 0)
            newSpeed /= speed;

        playerVelocity *= newSpeed;
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

        // We use the fromUpToGroundNormal quaternion to rotate our move direction along any surface we are on
        // If we are not on a surface this quaternion should default to identity to cover any oddities
        playerVelocity += accSpeed * (fromUpToGroundNormal * moveDirection);
    }

    void GroundMovement()
    {
        Vector3 desiredDirection;
        float desiredSpeed;

        // If there is a jump queued in the air then don't apply friction as it will cause stutters in momentum
        if (inputManager.queuedJump)
            ApplyFriction(0.0f);
        else
            ApplyFriction(1.0f);

        desiredDirection = CalculateRawDesiredDirection().normalized;

        moveDirection = desiredDirection;

        desiredSpeed = desiredDirection.magnitude * movementSpeed;
        Accelerate(desiredDirection, desiredSpeed, groundAcceleration);
    }

    void AirControl(Vector3 desiredDirection, float desiredSpeed)
    {
        float downSpeed, speed;
        float dot;
        float k;

        // If not moving in a forward/backwards direction, no control
        if (Mathf.Abs(inputManager.movementInput.y) < minimumSpeedForAirControl || Mathf.Abs(desiredSpeed) < minimumSpeedForAirControl)
            return;

        // This is to preserve the player's y velocity through calculations
        downSpeed = playerVelocity.y;
        playerVelocity.y = 0;

        speed = playerVelocity.magnitude;
        playerVelocity.Normalize();

        dot = Vector3.Dot(playerVelocity, desiredDirection);
        k = 32;
        k *= airControlPrecision * dot * dot * Time.deltaTime;

        // Allows changing directions while not speeding up
        if(dot > 0)
        {
            float xVel = playerVelocity.x * speed + desiredDirection.x * k;
            float zVel = playerVelocity.z * speed + desiredDirection.z * k;

            playerVelocity = new Vector3(xVel, 0, zVel);
            playerVelocity.Normalize();
            moveDirection = playerVelocity;
        }

        playerVelocity.x *= speed;
        playerVelocity.y = downSpeed; 
        playerVelocity.z *= speed;
    }

    void AirMovement()
    {
        Vector3 desiredDirection;
        float desiredSpeed;
        float desiredSpeedTwo;
        float acceleration;

        desiredDirection = CalculateRawDesiredDirection();

        desiredSpeed = desiredDirection.magnitude * movementSpeed;

        // Notice how we normalize the movement vector AFTER calculating the magnitude, this is what will make jumping at angles faster
        desiredDirection.Normalize();
        moveDirection = desiredDirection;
        

        // Air Control (CPM - Full Air Control)
        if (Vector3.Dot(playerVelocity, desiredDirection) < 0)
            acceleration = airDeceleration;
        else
            acceleration = airAcceleration;

        // If not holding forward/backward, but pressing a strafe ( horizontal key ) then we use the strafe speed/acceleration values
        desiredSpeedTwo = desiredSpeed;
        if (inputManager.movementInput.y == 0 && inputManager.movementInput.x != 0)
        {
            if (desiredSpeed > sideStrafeSpeed)
                desiredSpeed = sideStrafeSpeed;
            acceleration = sideStrafeAcceleration;
        }

        Accelerate(desiredDirection, desiredSpeed, acceleration);

        if (airControlPrecision > 0)
            AirControl(desiredDirection, desiredSpeedTwo);
    }

    void GroundWorldForces()
    {
        //float slopeAngle = Vector3.Angle(transform.up, groundHit.normal);
        float downAlignment = Vector3.Dot(-transform.up, downSlopeVector);
        float gravityComponent = slopeDescentMultiplier * downAlignment;
        Vector3 gravityForce = gravityComponent * downSlopeVector;
        playerVelocity += gravityForce;

        if(debugVectors)
            Debug.DrawRay(transform.position, gravityForce, Color.magenta);

        if (inputManager.queuedJump)
        {
            playerVelocity.y = jumpSpeed;
            inputManager.queuedJump = false;
        }
    }
    
    void AirWorldForces()
    {
        playerVelocity.y -= gravityStrength * Time.deltaTime;
    }

    void Movement()
    {
        if (charController.isGrounded)
            playerGrounded = true;

        if (playerGrounded)
        {
            GroundMovement();
            GroundWorldForces();
        }
        else if (!playerGrounded)
        {
            AirMovement();
            AirWorldForces();
        }

        if (debugVectors)
        {
            Debug.DrawRay(transform.position, surfaceRaycastLength * new Vector3(0, -1, 0));
            Debug.DrawRay(transform.position, fromUpToGroundNormal * moveDirection, Color.green);
            Debug.DrawRay(transform.position, playerVelocity, Color.red);
        }

        charController.Move(playerVelocity * Time.deltaTime);
        playerCamera.transform.position = transform.position + cameraOffset;
    }

    #endregion
}
