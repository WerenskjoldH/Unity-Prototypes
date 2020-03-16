using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    public float requiredTimeToFire = 1.0f;
    float timeHeldDown = 0;
    public bool mayAttack = false;
    public bool triggerFire = false;

    public float beamFadeTime = 0.25f;
    // Distance to draw beam end point when player misses
    public float beamSkyDrawMagnitude = 10.0f;
    public LineRenderer lineRenderer;
    Color lineRendererDefaultStartColor;
    Color lineRendererDefaultEndColor;
    public ParticleSystem eyeChargeParticles;
    public ParticleSystem eyeChargedParticles;

    public GameObject beamHitPosition;
    ParticleSystem beamHitParticles;
    
    
    private void Start()
    {
        eyeDefaultPosition = towerEye.transform.position;
        eyeRestPosition = eyeDefaultPosition;
        towerEyeRB = towerEye.GetComponent<Rigidbody2D>();

        lineRendererDefaultStartColor = lineRenderer.startColor;
        lineRendererDefaultEndColor = lineRenderer.endColor;
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

    IEnumerator FadeOutBeam()
    {
        float timePassed = 0;
        Color startColor = lineRenderer.startColor;
        Color endColor = lineRenderer.endColor;
        
        while (timePassed < beamFadeTime)
        {
            startColor.a = Mathf.Lerp(1, 0, timePassed/beamFadeTime);
            endColor.a = Mathf.Lerp(1, 0, timePassed/beamFadeTime);

            lineRenderer.startColor = startColor;
            lineRenderer.endColor = endColor;

            timePassed += Time.deltaTime;
            yield return null;
        }

        lineRenderer.enabled = false;
    }

    void FireBeam()
    {
        Vector3 beamDirection = (mouseGameObject.transform.position - towerEye.transform.position).normalized;
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

            beamHitPosition.transform.position = hitPoint;
            beamHitPosition.GetComponent<ParticleSystem>().Play();

            lineRenderer.SetPosition(1, hitPoint);
        }
        else
        {
            lineRenderer.SetPosition(1, towerEye.transform.position + beamSkyDrawMagnitude * beamDirection);
        }

        lineRenderer.startColor = lineRendererDefaultStartColor;
        lineRenderer.endColor = lineRendererDefaultEndColor;
        StartCoroutine(FadeOutBeam());
    }

    private void Update()
    {
        EyeMovement();

        if (Input.GetMouseButtonDown(0))
        {
            eyeChargeParticles.Play();
        }

        if (Input.GetMouseButton(0))
        {
            timeHeldDown += Time.deltaTime;
            if (!mayAttack && timeHeldDown >= requiredTimeToFire)
            {
                mayAttack = true;
                eyeChargedParticles.Play();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (mayAttack)
            {
                FireBeam();
            }
            eyeChargeParticles.Clear();
            eyeChargedParticles.Stop();
            timeHeldDown = 0;
            mayAttack = false;
        }

    }
}
