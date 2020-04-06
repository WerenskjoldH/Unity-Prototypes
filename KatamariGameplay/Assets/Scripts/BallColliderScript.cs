using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallColliderScript : MonoBehaviour
{
    [SerializeField]
    BallControllerScript controllerScript;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if(collision.gameObject.tag == "Collectable")
        {
            // New Approach: Check other objects size/weight/surface area and determine if it should stick or not


            collision.rigidbody.isKinematic = true;
            collision.collider.enabled = false;
            collision.transform.parent = gameObject.transform;
            CollidableScript otherCollideScript = collision.gameObject.GetComponent<CollidableScript>();
            controllerScript.IncreaseSize(otherCollideScript.sizeIncrease);
            otherCollideScript.RetainSize(controllerScript.GetSize());
        }
    }
}
