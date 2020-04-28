// Created via Acacia Developer FPS Controller How-To Series ( with modifications where I found necessary )

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;

    [SerializeField] private float movementSpeedVertical;
    [SerializeField] private float movementSpeedHorizontal;
    [SerializeField] private float movementSpeedDiagonal;

    [SerializeField] private bool runEnabled = true;
    [SerializeField] private float runSpeedMultiplier;
    [SerializeField] private float runBuildUp;
    [SerializeField] private KeyCode runKey;

    [SerializeField] private float slopeForce;
    [SerializeField] private float slopeForceRayLength;

    [SerializeField] private bool enableJumping;
    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private KeyCode jumpKey;

    private CharacterController charController;

    private bool isJumping, isRunning;

    private float movementSpeed;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float horizInput = Input.GetAxis(horizontalInputName);
        float vertInput = Input.GetAxis(verticalInputName);        

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;

        charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * movementSpeed);

        if((vertInput != 0 || horizInput != 0) && OnSlope())
            charController.Move(Vector3.down * charController.height / 2 * slopeForce);

        SetMovementSpeed(horizInput, vertInput);

        JumpInput();
    }

    private void SetMovementSpeed(float horizInput, float vertInput)
    {
        if(Input.GetKey(runKey))
        {
            if (horizInput != 0 && vertInput == 0)
                movementSpeed = Mathf.Lerp(movementSpeed, movementSpeedHorizontal * runSpeedMultiplier, Time.deltaTime * runBuildUp);
            else if (horizInput != 0 && vertInput != 0)
                movementSpeed = Mathf.Lerp(movementSpeed, movementSpeedDiagonal * runSpeedMultiplier, Time.deltaTime * runBuildUp);
            else if (horizInput == 0 && vertInput != 0)
                movementSpeed = Mathf.Lerp(movementSpeed, movementSpeedHorizontal * runSpeedMultiplier, Time.deltaTime * runBuildUp);
        }
        else
        {
            if (horizInput != 0 && vertInput == 0)
                movementSpeed = Mathf.Lerp(movementSpeed, movementSpeedHorizontal, Time.deltaTime * runBuildUp);
            else if (horizInput != 0 && vertInput != 0)
                movementSpeed = Mathf.Lerp(movementSpeed, movementSpeedDiagonal, Time.deltaTime * runBuildUp);
            else if (horizInput == 0 && vertInput != 0)
                movementSpeed = Mathf.Lerp(movementSpeed, movementSpeedHorizontal, Time.deltaTime * runBuildUp);
        }
    }

    private bool OnSlope()
    {
        if(isJumping)
            return false;

        RaycastHit hit;

        if(Physics.Raycast(transform.position, Vector3.down, out hit, charController.height / 2 * slopeForceRayLength))
            if(hit.normal != Vector3.up)
            {
                return true;
            }

        return false;
    }

    private void JumpInput()
    {
        if(Input.GetKeyDown(jumpKey) && !isJumping && enableJumping)
        {
            isJumping = true;

            StartCoroutine(JumpEvent());
        }
    }

    private IEnumerator JumpEvent()
    {
        charController.slopeLimit = 90.0f;

        float timeInAir = 0.0f;

        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);

            timeInAir += Time.deltaTime;

            yield return null;
        } while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);


        isJumping = false;
        charController.slopeLimit = 45.0f;
    }
}
