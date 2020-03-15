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
    Vector3 eyeRestPosition;
    public float maxEyeSpeed = 1.0f;
    public float maxEyeOffset = 0.5f;
    public float returnForce = 1.0f;
    public float bobHeight = 0.12f;
    public float recoilForce = 200.0f;

    float requiredTimeToFire = 1.0f;
    float timeHeldDown = 0;
    public bool mayAttack = false;
    public bool triggerFire = false;

    public EyeChargeParticlesScript eyeChargeParticleScript;

    private void Start()
    {
        eyeDefaultPosition = towerEye.transform.position;
        eyeRestPosition = eyeDefaultPosition;
        towerEyeRB = towerEye.GetComponent<Rigidbody2D>();
    }

    void EyeBobMovement()
    {
        eyeRestPosition = eyeDefaultPosition + new Vector3(0, bobHeight * Mathf.Sin(Time.timeSinceLevelLoad));
    }

    void EyeMovement()
    {
        Vector2 restVector = eyeRestPosition - towerEye.transform.position;
        Vector2 force;
        float restDistance = restVector.magnitude;
        float speed = maxEyeSpeed;
        restVector.Normalize();

        if (restDistance < 0.2)
        {
            speed = (restDistance - 0) / (speed - 0) * (maxEyeSpeed - 0);
        }

        restVector *= speed;
        force = restVector - towerEyeRB.velocity;
        towerEyeRB.AddForce(force);
        EyeBobMovement();
    }

    void EyeRecoilForce(float strength)
    {
        Vector2 force = strength * (eyeRestPosition - mouseGameObject.transform.position).normalized;

        towerEyeRB.AddForce(force);
    }

    private void Update()
    {
        EyeMovement();

        if (Input.GetMouseButtonDown(0))
        {
            eyeChargeParticleScript.PlayChargeEffect();
        }

        if (Input.GetMouseButton(0))
        {
            timeHeldDown += Time.deltaTime;
            if (timeHeldDown >= requiredTimeToFire)
                mayAttack = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (mayAttack)
            {
                EyeRecoilForce(recoilForce);
            }
            eyeChargeParticleScript.StopChargeEffect();
            timeHeldDown = 0;
            mayAttack = false;
        }

    }
}
