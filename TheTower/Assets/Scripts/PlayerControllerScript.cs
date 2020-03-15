using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    [SerializeField]
    GameObject towerBase;
    [SerializeField]
    GameObject towerEye;
    Rigidbody2D towerEyeRB;
    [SerializeField]
    GameObject mouseGameObject;

    Vector3 eyeDefaultPosition;
    public float maxEyeSpeed = 1.0f;
    public float maxEyeOffset = 0.5f;
    public float returnForce = 1.0f;

    private void Start()
    {
        eyeDefaultPosition = towerEye.transform.position;
        towerEyeRB = towerEye.GetComponent<Rigidbody2D>();
    }

    void EyeMovement()
    {
        Vector2 restVector = eyeDefaultPosition - towerEye.transform.position;
        float restDistance = restVector.magnitude;
        float speed = maxEyeSpeed;
        restVector.Normalize();

        if (restDistance < 0.2)
        {
            speed = (restDistance - 0) / (speed - 0) * (maxEyeSpeed - 0);
        }

        restVector *= speed;
        Vector2 force = restVector - towerEyeRB.velocity;
        towerEyeRB.AddForce(force);
    }

    void EyeRecoilForce(float strength)
    {
        Vector2 force = strength * (eyeDefaultPosition - mouseGameObject.transform.position).normalized;

        towerEyeRB.AddForce(force);
    }

    private void Update()
    {
        EyeMovement();

        if (Input.GetMouseButtonUp(0))
            EyeRecoilForce(250.0f);
    }
}
