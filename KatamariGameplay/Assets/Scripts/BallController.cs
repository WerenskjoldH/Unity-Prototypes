using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{

    float rotation;

    [Header("Transforms and Rigidbodies")]
    [SerializeField]
    Rigidbody collidingSphere;
    [SerializeField]
    Transform steeringObject;

    [Header("Driving Properties")]
    public float accelerationForce;
    public float turningSpeed;

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
            collidingSphere.GetComponent<Rigidbody>().AddForce(10.0f * steeringObject.transform.forward);
        else if (Input.GetKey(KeyCode.S))
            collidingSphere.GetComponent<Rigidbody>().AddForce(-10.0f * steeringObject.transform.forward);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.D))
            rotation += turningSpeed;
        else if (Input.GetKey(KeyCode.A))
            rotation -= turningSpeed;

        transform.position = collidingSphere.transform.position;

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + rotation, 0);
        steeringObject.Rotate(Vector3.up, rotation);
        rotation = 0;
    }
}
