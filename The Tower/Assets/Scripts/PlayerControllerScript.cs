using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerControllerScript : MonoBehaviour
{
    public GameObject towerBase;
    [SerializeField]
    GameObject towerEye;
    Rigidbody2D towerEyeRB;
    [SerializeField]
    PlayerMouseScript mouseScript;

    Vector3 eyeDefaultPosition;
    Vector3 eyeRestPosition;
    [SerializeField]
    float maxEyeSpeed = 1.0f;
    [SerializeField]
    float maxEyeOffset = 0.5f;
    [SerializeField]
    float returnForce = 1.0f;
    [SerializeField]
    float bobHeight = 0.12f;
    [SerializeField]
    float recoilForce = 200.0f;
    [SerializeField]
    float beamBlastRadius = 1.0f;

    [SerializeField]
    float requiredTimeToFire = 1.0f;
    [SerializeField]
    bool mayAttack = false;
    [SerializeField]
    bool triggerFire = false;

    [SerializeField]
    float beamFadeTime = 0.25f;
    // Distance to draw beam end point when player misses
    [SerializeField]
    float beamSkyDrawMagnitude = 10.0f;
    [SerializeField]
    LineRenderer lineRenderer;
    Color lineRendererDefaultStartColor;
    Color lineRendererDefaultEndColor;
    [SerializeField]
    ParticleSystem eyeChargeParticles;
    [SerializeField]
    ParticleSystem eyeChargedParticles;

    [SerializeField]
    GameObject beamHitPositionObject;
    ParticleSystem beamHitParticles;

    private void Start()
    {
        eyeDefaultPosition = towerEye.transform.position;
        eyeRestPosition = eyeDefaultPosition;
        towerEyeRB = towerEye.GetComponent<Rigidbody2D>();

        lineRendererDefaultStartColor = lineRenderer.startColor;
        lineRendererDefaultEndColor = lineRenderer.endColor;
    }

    private void Update()
    {
        

        if (mouseScript.GetMouseDown())
        {
            eyeChargeParticles.Play();
        }

        if (mouseScript.GetMouseHold())
        {
            if (!mayAttack && mouseScript.GetMouseHoldDuration() >= requiredTimeToFire)
            {
                mayAttack = true;
                eyeChargedParticles.Play();
            }
        }

        if (mouseScript.GetMouseUp())
        {
            if (mayAttack)
            {
                FireBeam();
            }
            //eyeChargeParticles.Clear();
            eyeChargedParticles.Stop();
            mayAttack = false;
        }

    }

    private void FixedUpdate()
    {
        EyeMovement();
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
        Vector2 force = strength * (eyeRestPosition - mouseScript.GetMousePosition()).normalized;

        towerEyeRB.AddForce(force);
    }

    IEnumerator FadeOutBeam()
    {
        float timePassed = 0;
        Color startColor = lineRenderer.startColor;
        Color endColor = lineRenderer.endColor;

        while (timePassed < beamFadeTime)
        {
            startColor.a = Mathf.Lerp(1, 0, timePassed / beamFadeTime);
            endColor.a = Mathf.Lerp(1, 0, timePassed / beamFadeTime);

            lineRenderer.startColor = startColor;
            lineRenderer.endColor = endColor;

            timePassed += Time.deltaTime;
            yield return null;
        }

        lineRenderer.enabled = false;
    }

    void FireBeam()
    {
        Vector3 beamDirection = (mouseScript.transform.position - towerEye.transform.position).normalized;
        beamDirection.z = 0;
        RaycastHit2D hit = Physics2D.Raycast(towerEye.transform.position, beamDirection);

        EyeRecoilForce(recoilForce);

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, towerEye.transform.position);

        if (hit.collider != null)
        {
            Vector3 hitPoint = hit.point;
            hitPoint.z = 0;
            //float distance = (hitPoint - towerEye.transform.position).magnitude;

            beamHitPositionObject.transform.position = hitPoint;
            beamHitPositionObject.GetComponent<ParticleSystem>().Play();

            lineRenderer.SetPosition(1, hitPoint);

            // Find and inflict damage on hit units
            Collider2D[] colliders = Physics2D.OverlapCircleAll(hitPoint, beamBlastRadius);
            int i = 0;
            while (i < colliders.Length)
            {
                if (colliders[i].tag != "Attacker")
                {
                    i++;
                    continue;
                }

                colliders[i].GetComponent<AttackerInterface>().Hit();
                i++;
            }
        }
        else
        {
            lineRenderer.SetPosition(1, towerEye.transform.position + beamSkyDrawMagnitude * beamDirection);
        }

        lineRenderer.startColor = lineRendererDefaultStartColor;
        lineRenderer.endColor = lineRendererDefaultEndColor;
        StartCoroutine(FadeOutBeam());
    }
}
