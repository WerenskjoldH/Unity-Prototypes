using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallColliderScript : MonoBehaviour
{
    [SerializeField]
    BallControllerScript controllerScript;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if(other.gameObject.tag == "Collectable")
        {
            // New Approach: Check other objects size/weight/surface area and determine if it should stick or not

            //Destroy(collision.rigidbody);
            //collision.rigidbody.isKinematic = true;
            //other.GetComponent<Collider>().enabled = false;
            other.transform.parent = gameObject.transform;
            CollidableScript otherCollideScript = other.gameObject.GetComponent<CollidableScript>();
            controllerScript.IncreaseSize(otherCollideScript.sizeIncrease);
            otherCollideScript.RetainSize(controllerScript.GetSize());
        }
    }
}
