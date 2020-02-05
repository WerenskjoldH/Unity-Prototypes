using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float launchPower = 1000.0f;

    bool stuckToSurface = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Collidable")
        {
            Debug.Log("Hit");
            stuckToSurface = true;
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    void Start()
    {
        gameObject.GetComponent<LineRenderer>().positionCount = 2;
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (stuckToSurface && Input.GetMouseButton(0))
        {
            LineRenderer lr = gameObject.GetComponent<LineRenderer>();
            lr.enabled = true;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, mousePos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            gameObject.GetComponent<LineRenderer>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            gameObject.GetComponent<Rigidbody2D>().AddForce( 100.0f * (mousePos - transform.position));
        }
    }
}
