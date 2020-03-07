﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    // Both Launch Properties Use World Units
    public float launchPower = 50.0f;
    public float maxLaunchMult = 3.0f;
    public float requiredGrindTimeToRefillJumps = 0.1f;
    public int totalJumpsAllowed = 3;
    public JumpCounterScript jumpCounterScript;
    public GameObject parentObject;
    public ParticleSystem collisionParticleEffect;
    
    int totalJumpsMade = 0;
    float grindTime = 0;
    int grindParticleObjects = 0;
    bool stuckToSurface = false;
    bool jumpCancel = false;
    Collision2D lastCollision = null;

    public bool IsStuckToSurface()
    {
        return stuckToSurface;
    }

    public GameObject ObjectStuckTo()
    {
        return lastCollision.gameObject;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        parentObject.transform.SetParent(collision.transform);

        // Can probably make this more juicy in the future
        if (collision.relativeVelocity.magnitude > 14)
        {
            Instantiate(collisionParticleEffect, transform.position, Quaternion.identity);
            Instantiate(collisionParticleEffect, transform.position, Quaternion.identity);
        }
        else if (collision.relativeVelocity.magnitude > 8)
            Instantiate(collisionParticleEffect, transform.position, Quaternion.identity);

        lastCollision = collision;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        // Grinding Particle Effect
        if (!stuckToSurface && collision.relativeVelocity.magnitude > 3.0f && Input.GetMouseButton(0))
        {
            if (grindTime / 0.1f > grindParticleObjects)
            {
                Instantiate(collisionParticleEffect, transform.position, Quaternion.identity);
                grindParticleObjects++;
            }

            if(totalJumpsMade > 0 && grindTime >= requiredGrindTimeToRefillJumps)
            {
                totalJumpsMade = 0;
                jumpCounterScript.ResetJumps();
            }

            grindTime += Time.deltaTime;
        }
        lastCollision = collision;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        parentObject.transform.SetParent(null);

        if (lastCollision != null && collision.collider == lastCollision.collider)
            lastCollision = null;

        if (grindTime > 0)
        {
            Debug.Log("Grinded for: " + grindTime + "Second(s)");
            grindParticleObjects = 0;
            grindTime = 0;
        }
        
    }

    private void CollisionStick(Collision2D collision)
    {
        if (collision != null && collision.gameObject.tag == "Collidable")
        {
            stuckToSurface = true;
            lastCollision = collision;
            totalJumpsMade = 0;
            jumpCounterScript.ResetJumps();
            FixedJoint2D joint = gameObject.GetComponent<FixedJoint2D>();
            joint.enabled = true;
            joint.connectedAnchor = collision.transform.InverseTransformPoint(gameObject.transform.position);
            joint.connectedBody = collision.rigidbody;
        }
    }

    void Start()
    {
        gameObject.GetComponent<LineRenderer>().positionCount = 2;
        jumpCounterScript.SetNumberOfJumps(totalJumpsAllowed);

        if(GetComponent<TrailRenderer>() != null)
            GetComponent<TrailRenderer>().startWidth = gameObject.GetComponent<SpriteRenderer>().size.y;
        
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        if (totalJumpsMade < totalJumpsAllowed && !jumpCancel)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 powerLine = (mousePos - transform.position);
                float powerLineMagnitude = Mathf.Min(powerLine.magnitude, maxLaunchMult);
                powerLine.Normalize();


                LineRenderer lr = gameObject.GetComponent<LineRenderer>();
                lr.enabled = true;
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, transform.position + powerLineMagnitude * powerLine);
            }

            if (Input.GetMouseButtonUp(0))
            {
                stuckToSurface = false;
                totalJumpsMade++;
                jumpCounterScript.ReduceJumps();

                gameObject.GetComponent<LineRenderer>().enabled = false;

                gameObject.GetComponent<FixedJoint2D>().enabled = false;

                Vector2 differenceVector = (mousePos - transform.position);
                Vector2 launchForce = launchPower * Mathf.Min(differenceVector.magnitude, maxLaunchMult) * differenceVector.normalized;
                gameObject.GetComponent<Rigidbody2D>().AddForce(launchForce);
            }
        }

        if (Input.GetMouseButton(1))
        {
            CollisionStick(lastCollision);
        }

        if (Input.GetMouseButton(0) && Input.GetMouseButtonDown(1))
        {
            jumpCancel = true;
            gameObject.GetComponent<LineRenderer>().enabled = false;
        }
        else if (Input.GetMouseButtonUp(0))
            jumpCancel = false;
    }
}