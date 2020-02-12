using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    Transform startPosition;

    void Start()
    {
        startPosition = gameObject.transform;
    }

    void Update()
    {
        Vector2 newPos = startPosition.position;
        newPos.y = Mathf.Sin(Time.realtimeSinceStartup);
        gameObject.transform.position = newPos;
    }
}
