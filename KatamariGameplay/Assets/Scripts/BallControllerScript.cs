using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class BallControllerScript : MonoBehaviour
{

    float steeringRotation;
    float currentSize = 0;
    float cameraIncreaseFactor = 0.9f;

    [Header("Transforms and Rigidbodies")]
    [SerializeField]
    Rigidbody ballCollidingSphere;
    [SerializeField]
    Transform itemCollectionTransform;
    [SerializeField]
    Transform steeringObject;
    [SerializeField]
    CinemachineVirtualCamera playerCamera;

    [Header("Driving Properties")]
    public float torqueForce = 10.0f;
    public float turningSpeed;

    public float GetSize()
    {
        return currentSize;
    }

    public void IncreaseSize(float f)
    {
        currentSize += f;
        //itemCollectionTransform.localScale = new Vector3(currentSize, currentSize, currentSize);
        itemCollectionTransform.DOScale(new Vector3(currentSize, currentSize, currentSize), 0.5f);
        CinemachineTransposer transposer = playerCamera.GetCinemachineComponent<CinemachineTransposer>();
        DOTween.To(
            () => transposer.m_FollowOffset, 
            x => transposer.m_FollowOffset = x,
            transposer.m_FollowOffset + new Vector3(0, cameraIncreaseFactor*f, -(cameraIncreaseFactor * f)), 
            0.25f)
            .SetEase(Ease.OutCubic);
    }

    private void Start()
    {
        currentSize += ballCollidingSphere.gameObject.transform.localScale.x;
        itemCollectionTransform.localScale = new Vector3(currentSize, currentSize, currentSize);
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
            ballCollidingSphere.GetComponent<Rigidbody>().AddTorque(steeringObject.transform.right * torqueForce);
        else if (Input.GetKey(KeyCode.S))
            ballCollidingSphere.GetComponent<Rigidbody>().AddTorque(-steeringObject.transform.right * torqueForce);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.D))
            steeringRotation += turningSpeed;
        else if (Input.GetKey(KeyCode.A))
            steeringRotation -= turningSpeed;

        if (Input.GetKeyDown(KeyCode.Space))
            IncreaseSize(0.2f);

        transform.position = ballCollidingSphere.transform.position;

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + steeringRotation, 0);
        steeringObject.Rotate(Vector3.up, steeringRotation);
        steeringRotation = 0;
    }
}
