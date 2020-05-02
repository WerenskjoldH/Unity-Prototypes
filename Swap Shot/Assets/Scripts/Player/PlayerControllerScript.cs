﻿using System.Collections;
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

    [SerializeField] float airControlPrecision = 0.3f;
    [SerializeField] float airAcceleration = 2;
    [SerializeField] float airDeceleration = 2;
    [SerializeField] float sideStrafeSpeed = 1;
    [SerializeField] float sideStrafeAcceleration = 50;
    [SerializeField] float jumpSpeed = 8;
    bool queuedJump = false;

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
        // We shouldn't be able to do front/back tucks, so lock that pitch between target values
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

        if (queuedJump)
            ApplyFriction(0.0f);
        else
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

        if(queuedJump)
        {
            playerVelocity.y = jumpSpeed;
            queuedJump = false;
        }
    }

    void QueueJump()
    {
        if (Input.GetButtonDown("Jump") && !queuedJump)
            queuedJump = true;
        if (Input.GetButtonUp("Jump"))
            queuedJump = false;
    }

    void AirControl(Vector3 desiredDirection, float desiredSpeed)
    {
        float zSpeed, speed;
        float dot;
        float k;

        // If not moving in a forward/backwards direction, no control
        if (Mathf.Abs(inputManager.movementInput.y) < 0.001 || Mathf.Abs(desiredSpeed) < 0.001)
            return;

        zSpeed = playerVelocity.y;
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
            float yVel = playerVelocity.y * speed + desiredDirection.y * k;
            float zVel = playerVelocity.z * speed + desiredDirection.z * k;

            playerVelocity = new Vector3(xVel, yVel, zVel);
            playerVelocity.Normalize();
            moveDirection = playerVelocity;
        }

        playerVelocity.x *= speed;
        // zSpeed is named such since ID's world unit coordinates being oriented differently
        playerVelocity.y = zSpeed; 
        playerVelocity.z *= speed;
    }

    void AirMovement()
    {
        Vector3 desiredDirection;
        float desiredVelocity = airAcceleration;
        float acceleration;

        desiredDirection = new Vector3(inputManager.movementInput.x, 0, inputManager.movementInput.y);
        desiredDirection = transform.TransformDirection(desiredDirection);

        float desiredSpeed = desiredDirection.magnitude * movementSpeed;

        desiredDirection.Normalize();
        moveDirection = desiredDirection;

        // Air Control (CPM - Full Air Control)
        float desiredSpeedTwo = desiredSpeed;
        if (Vector3.Dot(playerVelocity, desiredDirection) < 0)
            acceleration = airDeceleration;
        else
            acceleration = airAcceleration;

        if(inputManager.movementInput.y == 0 && inputManager.movementInput.x != 0)
        {
            if (desiredSpeed > sideStrafeSpeed)
                desiredSpeed = sideStrafeSpeed;
            acceleration = sideStrafeAcceleration;
        }

        Debug.Log(desiredDirection);
        Accelerate(desiredDirection, desiredSpeed, acceleration);

        if (airControlPrecision > 0)
            AirControl(desiredDirection, desiredSpeedTwo);

        playerVelocity.y -= gravityStrength * Time.deltaTime;
    }

    void Movement()
    {
        QueueJump();
        if (charController.isGrounded)
            GroundMovement();
        else if(!charController.isGrounded)
            AirMovement();

        charController.Move(playerVelocity * Time.deltaTime);
        playerCameraTransform.position = transform.position + cameraOffset;
    }
}
