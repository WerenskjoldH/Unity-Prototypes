using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    [SerializeField] GameObject TargetBody;
    [SerializeField] Vector3 bodyOffset;

    public void Update()
    {
        gameObject.transform.position = TargetBody.transform.position + bodyOffset;
    }
}
