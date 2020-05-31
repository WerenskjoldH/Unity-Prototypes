using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LinearPlatformMovementScript : MonoBehaviour
{
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

        collision.gameObject.GetComponent<PlayerControllerScript>().AddToExternalVelocity(platformVelocity);
    }

    private void Start()
    {
        targetPoint = startPoint;
        transform.DOMove(targetPoint.position, movementSpeed).SetEase(Ease.InOutCubic);
    }

    private void FixedUpdate()
    {
        platformVelocity = (transform.position - previousPosition);
        previousPosition = transform.position;
    }

    void Update()
    {
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
