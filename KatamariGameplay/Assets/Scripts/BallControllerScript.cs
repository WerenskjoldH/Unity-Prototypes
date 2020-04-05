using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControllerScript : MonoBehaviour
{

    float rotation;
    float currentSize = 2;

    [Header("Transforms and Rigidbodies")]
    [SerializeField]
    Rigidbody ballCollidingSphere;
    [SerializeField]
    Transform itemCollectionTransform;
    [SerializeField]
    Transform steeringObject;

    [Header("Driving Properties")]
    public float accelerationForce;
    public float turningSpeed;

    public float GetSize()
    {
        return currentSize;
    }

    public void IncreaseSize(float f)
    {
        currentSize += f;
        itemCollectionTransform.localScale = new Vector3(currentSize, currentSize, currentSize);
    }

    private void Start()
    {
        currentSize += ballCollidingSphere.gameObject.transform.localScale.x;
        itemCollectionTransform.localScale = new Vector3(currentSize, currentSize, currentSize);
        Debug.Log(currentSize);
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
            ballCollidingSphere.GetComponent<Rigidbody>().AddForce(10.0f * steeringObject.transform.forward);
        else if (Input.GetKey(KeyCode.S))
            ballCollidingSphere.GetComponent<Rigidbody>().AddForce(-10.0f * steeringObject.transform.forward);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.D))
            rotation += turningSpeed;
        else if (Input.GetKey(KeyCode.A))
            rotation -= turningSpeed;

        transform.position = ballCollidingSphere.transform.position;

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + rotation, 0);
        steeringObject.Rotate(Vector3.up, rotation);
        rotation = 0;
    }
}
