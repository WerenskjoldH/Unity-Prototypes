// Created via Acacia Developer FPS Controller How-To Series ( with modifications where I found necessary )

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private string mouseXInputName, mouseYInputName;

    [SerializeField] private float mouseSensitivity;

    [SerializeField] private float positiveYThreshould = 90.0f, negativeYThreshould = -90.0f;

    [SerializeField] private Transform playerBody;

    private float xAxisClamp;


    private void Awake()
    {
        LockCursor();
        xAxisClamp = 0;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        float mouseX = Input.GetAxis(mouseXInputName) * mouseSensitivity;
        float mouseY = Input.GetAxis(mouseYInputName) * mouseSensitivity;

        xAxisClamp += mouseY;

        if (xAxisClamp > positiveYThreshould)
        {
            xAxisClamp = positiveYThreshould;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(360 - positiveYThreshould);
        }
        else if (xAxisClamp < negativeYThreshould)
        {
            xAxisClamp = negativeYThreshould;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(-1.0f * negativeYThreshould);
        }

        transform.Rotate(Vector3.left * mouseY);

        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void ClampXAxisRotationToValue(float v)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = v;
        transform.eulerAngles = eulerRotation;
    }
}
