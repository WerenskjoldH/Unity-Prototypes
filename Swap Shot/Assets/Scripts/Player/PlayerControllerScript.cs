using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using DG.Tweening;

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



// This controller takes inspiration from WiggleWizard's implementation ( WiggleWizard ported a nice replica of Quake 3's movement )

/*
    Things Todo & Try:
        - Collisions with the player while in the air should stop their velocity (or at least dampen it), right now things act as if there was no impact
            + This is an easy fix, I just want to see if this could perhaps be kind of fun to mess with

        - Migrate HUD To It's Own Script, probably
*/
public class PlayerControllerScript : MonoBehaviour
{
    [Header("Script References")]

    LevelManagerScript levelManagerScript;

    [Space(5)]
    [Header("Camera")]
    [SerializeField] CinemachineVirtualCamera playerCamera;
    [SerializeField] Vector3 cameraOffset = Vector3.zero;
    [Space(5)]

    [Header("Look & Movement")]
    Vector3 playerVelocity = Vector3.zero;
    // This is primarily for debugging
    Vector3 moveDirection = Vector3.zero;

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

    RaycastHit groundHit;
    Quaternion fromUpToGroundNormal;
    Vector3 downSlopeVector;

    [SerializeField] float surfaceRaycastLength = 1.5f;
    bool prevPlayerGrounded = false;
    bool playerGrounded = false;

    CharacterController charController;

    [Header("Player Info")]
    bool isAlive = true;
    bool startInput = false;

    [Space(5)]

    [Header("Hud Options")]

    // We are assuming the width and height of the image are equivalent
    [SerializeField] Image crosshairUiImage;
    float defaultCrosshairSize;
    [SerializeField] float crosshairGrowthFactor = 1.2f;
    [SerializeField] float crosshairGrowShrinkSpeed = 0.5f;

    [SerializeField] TMP_Text speedTMP;
    [SerializeField] TMP_Text timeTMP;

    [Space(5)]

    [Header("Debug Options")]
    [SerializeField] bool debugVectors;

    InputManager inputManager;

    #region Getters & Setters

    public bool GetPlayerAlive()
    {
        return isAlive;
    }

    // This should not ever be used to set the player alive, ResetPlayer() is the safer alternative
    public void SetPlayerAlive(bool t)
    {
        isAlive = t;
    }

    public bool GetStartInput()
    {
        return startInput;
    }

    // The character controller overrides attempts to change position
    public void SetPosition(Vector3 p)
    {
        charController.enabled = false;
        transform.position = p;
        charController.enabled = true;
    }

    public void SetRotation(Quaternion q)
    {
        charController.enabled = false;
        playerCamera.transform.rotation = q;
        viewPitch = q.eulerAngles.x;
        transform.rotation = q;
        charController.enabled = true;
    }

    #endregion

    void Awake()
    {
        charController = GetComponent<CharacterController>();
        levelManagerScript = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManagerScript>();
        inputManager = new InputManager();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        defaultCrosshairSize = crosshairUiImage.rectTransform.localScale.x;
    }
   
    void Update()
    {
        inputManager.Update();
        PlayerBeginLevel();
        MouseLook();
        UpdateHud();
    }

    void FixedUpdate()
    {

        CheckGroundedSurface();
        Movement();
    }

    #region Player Status

    public void ResetPlayer()
    {
        isAlive = true;
        startInput = false;
    }

    void PlayerBeginLevel()
    {
        if (startInput)
            return;

        if (inputManager.movementInput != Vector2.zero)
            startInput = true;
    }

    #endregion

    #region HUD

    void GrowCrosshair()
    {
        float crosshairGrownSize = defaultCrosshairSize + crosshairGrowthFactor;
        crosshairUiImage.rectTransform.DOScale(new Vector3(crosshairGrownSize, crosshairGrownSize), crosshairGrowShrinkSpeed);
    }

    void ShrinkCrosshair()
    {
        crosshairUiImage.rectTransform.DOScale(new Vector3(defaultCrosshairSize, defaultCrosshairSize), crosshairGrowShrinkSpeed);
    }

    void UpdateHud()
    {
        int playerSpeed = (int)playerVelocity.magnitude;
        float levelTime;
        int minutes, seconds, milliseconds;

        levelTime = levelManagerScript.GetLevelTime();
        Debug.Log(levelTime);
        // Players will certainly take minutes, so we need to be prepped for that

        minutes = (int)(levelTime / 60.0f);
        levelTime -= minutes * 60.0f;
        seconds = (int)(levelTime % 60.0f);
        milliseconds = (int)((levelTime - seconds) * 100.0f);

        string minutesStr = minutes.ToString("00");
        string secondsStr = seconds.ToString("00");
        // We calculate this, but are intentionally not using it right now
        string millisecondsStr = milliseconds.ToString("00");
        string timeStr = string.Format("<mspace=0.5em>{0}:{1}</mspace>", minutesStr, secondsStr);

        speedTMP.text = playerSpeed.ToString();
        timeTMP.text = timeStr;
    }

    #endregion

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
            GrowCrosshair();
            inputManager.queuedJump = false;
            playerGrounded = false;
        }
    }
    
    void AirWorldForces()
    {
        playerVelocity.y -= gravityStrength * Time.deltaTime;
    }

    void StateChecks()
    {
        prevPlayerGrounded = playerGrounded;
    }

    void DebugVectors()
    {
        if (debugVectors)
        {
            Debug.DrawRay(transform.position, surfaceRaycastLength * new Vector3(0, -1, 0));
            Debug.DrawRay(transform.position, fromUpToGroundNormal * moveDirection, Color.green);
            Debug.DrawRay(transform.position, playerVelocity, Color.red);
        }
    }

    void Movement()
    {
        if (charController.isGrounded)
        {
            if (!prevPlayerGrounded)
                ShrinkCrosshair();

            playerGrounded = true;
        }

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

        DebugVectors();

        charController.Move(playerVelocity * Time.deltaTime);
        playerCamera.transform.position = transform.position + cameraOffset;
    }

    #endregion
}
