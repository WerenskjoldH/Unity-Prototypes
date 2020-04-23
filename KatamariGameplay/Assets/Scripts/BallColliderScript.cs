using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallColliderScript : MonoBehaviour
{
    [SerializeField]
    BallControllerScript controllerScript;
    [SerializeField]
    GameObject collectedObjectParent;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if(other.gameObject.tag == "Collectable")
        {
            // New Approach: Check other objects size/weight/surface area and determine if it should stick or not

            CollidableScript otherCollideScript = other.gameObject.GetComponent<CollidableScript>();
            Debug.Log(controllerScript.GetSize() + " " + other.GetComponent<CollidableScript>().GetSize());
            if (controllerScript.GetSize() < other.GetComponent<CollidableScript>().GetSize())
                return;

            otherCollideScript.audioSource.Play();
            other.GetComponent<Collider>().enabled = false;
            other.gameObject.layer = LayerMask.NameToLayer("Ball");
            other.transform.parent = collectedObjectParent.transform;
            controllerScript.IncreaseSize(otherCollideScript.GetSize());
        }
    }
}
