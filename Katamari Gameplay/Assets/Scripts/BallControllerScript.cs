using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class BallControllerScript : MonoBehaviour
{

    float steeringRotation;
    [SerializeField]
    float currentRadius = 0;
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

    [Header("Growth Properties")]
    [SerializeField]
    float inverseGrowthFactor = 16.0f;


    public float GetSize()
    {
        return 2.0f * currentRadius;
    }

    public void IncreaseSize(float size)
    {
        // This needs to be seriously re-worked
        float radInc = size / inverseGrowthFactor;

        currentRadius += radInc;
        //itemCollectionTransform.localScale = new Vector3(currentRadius, currentRadius, currentRadius);
        itemCollectionTransform.DOScale(new Vector3(currentRadius, currentRadius, currentRadius), 0.5f);
        CinemachineTransposer transposer = playerCamera.GetCinemachineComponent<CinemachineTransposer>();
        DOTween.To(
            () => transposer.m_FollowOffset, 
            x => transposer.m_FollowOffset = x,
            transposer.m_FollowOffset + new Vector3(0, cameraIncreaseFactor* radInc, -(cameraIncreaseFactor * radInc)), 
            0.25f)
            .SetEase(Ease.OutCubic);
        Debug.Log("Ball Weight: " + currentRadius);
        Debug.Log("Added " + size + " weight");
    }

    private void Start()
    {
        currentRadius += ballCollidingSphere.gameObject.transform.localScale.x;
        itemCollectionTransform.localScale = new Vector3(currentRadius, currentRadius, currentRadius);
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Rigidbody rb = ballCollidingSphere.GetComponent<Rigidbody>();
            rb.AddTorque(steeringObject.transform.right * torqueForce);
            rb.AddForce(steeringObject.transform.forward * torqueForce / 4.0f);
        }
        else if (Input.GetKey(KeyCode.S)) {
            Rigidbody rb = ballCollidingSphere.GetComponent<Rigidbody>();
            rb.AddTorque(-steeringObject.transform.right * torqueForce);
            rb.AddForce(-steeringObject.transform.forward * torqueForce / 4.0f);
        }
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
