using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollectedItemsScript : MonoBehaviour
{
    [SerializeField]
    GameObject ballCollider;

    void Update()
    {
        transform.position = ballCollider.GetComponent<Transform>().position;
        transform.rotation = ballCollider.GetComponent<Transform>().rotation;
    }
}
