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
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.GetComponent<Collider>().enabled = false;
            other.transform.parent = gameObject.transform;
            CollidableScript otherCollideScript = other.gameObject.GetComponent<CollidableScript>();
            controllerScript.IncreaseSize(otherCollideScript.sizeIncrease);
            otherCollideScript.RetainSize(controllerScript.GetSize());

        }
    }
}
