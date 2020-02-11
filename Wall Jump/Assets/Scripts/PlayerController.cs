using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Both Launch Properties Use World Units
    public float launchPower = 50.0f;
    public float maxLaunchMult = 3.0f;
    public JumpCounterScript jumpCounterScript;
    public int totalJumpsAllowed = 3;

    int totalJumpsMade = 0;
    bool stuckToSurface = false;
    Collision2D lastCollision = null;

    public bool IsStuckToSurface()
    {
        return stuckToSurface;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        lastCollision = collision;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider == lastCollision.collider)
            lastCollision = null;
    }

    private void CollisionStick(Collision2D collision)
    {
        if(collision != null && collision.gameObject.tag == "Collidable")
        {
            Debug.Log("Stick Triggered");
            stuckToSurface = true;
            lastCollision = collision;
            totalJumpsMade = 0;
            jumpCounterScript.ResetJumps();
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    void Start()
    {
        gameObject.GetComponent<LineRenderer>().positionCount = 2;
        jumpCounterScript.SetNumberOfJumps(totalJumpsAllowed);
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        if (totalJumpsMade < totalJumpsAllowed)
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

                gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                Vector2 differenceVector = (mousePos - transform.position);
                Vector2 launchForce = launchPower * Mathf.Min(differenceVector.magnitude, maxLaunchMult) * differenceVector.normalized;
                gameObject.GetComponent<Rigidbody2D>().AddForce(launchForce);
                

            }
        }

        if (Input.GetMouseButton(1) && !stuckToSurface)
        {
            CollisionStick(lastCollision);
        }
    }
}
