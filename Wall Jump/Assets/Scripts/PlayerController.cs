using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float launchPower = 100.0f;
    public JumpCounterScript jumpCounterScript;
    public int totalJumpsAllowed = 3;

    int totalJumpsMade = 0;
    bool stuckToSurface = false;
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Collidable")
        {
            stuckToSurface = true;
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

        if (totalJumpsMade < totalJumpsAllowed)
        {
            if (Input.GetMouseButton(0))
            {
                stuckToSurface = false;
                
                LineRenderer lr = gameObject.GetComponent<LineRenderer>();
                lr.enabled = true;
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, mousePos);
            }

            if (Input.GetMouseButtonUp(0))
            {
                totalJumpsMade++;
                jumpCounterScript.ReduceJumps();
                gameObject.GetComponent<LineRenderer>().enabled = false;
                gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                Vector2 diffVec = (mousePos - transform.position);
                gameObject.GetComponent<Rigidbody2D>().AddForce(launchPower * diffVec);
            }
        }
    }
}
