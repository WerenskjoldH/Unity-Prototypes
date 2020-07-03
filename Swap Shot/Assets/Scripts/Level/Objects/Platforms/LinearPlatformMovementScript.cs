using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LinearPlatformMovementScript : MonoBehaviour
{

    // TO-DO: Convert this to a list so we can have infinite points and at the end loop back to the first
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    Transform targetPoint;

    [SerializeField] float movementSpeed = 5.0f;

    Vector3 previousPosition;
    Vector3 platformVelocity;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
            return;

        // To ensure the player is effected by the ground they are standing on, we must apply the platform's velocity to the player as an external force
        collision.gameObject.GetComponent<PlayerControllerScript>().AddToExternalVelocity(platformVelocity);
    }

    private void Start()
    {
        targetPoint = startPoint;
        // This will tween the platform between its current position and target position
        transform.DOMove(targetPoint.position, movementSpeed).SetEase(Ease.InOutCubic);
    }

    private void FixedUpdate()
    {
        // Update the platform's velocity
        platformVelocity = (transform.position - previousPosition);
        previousPosition = transform.position;
    }

    void Update()
    {
        // When a target point is reached, we swap the target point and starting point
        if((targetPoint.position - transform.position).magnitude < 0.1f)
        {
            if (targetPoint == startPoint)
            {
                targetPoint = endPoint;
            }
            else
            {
                targetPoint = startPoint;
            }
            transform.DOMove(targetPoint.position, movementSpeed).SetEase(Ease.InOutCubic);
        }
    }
}
