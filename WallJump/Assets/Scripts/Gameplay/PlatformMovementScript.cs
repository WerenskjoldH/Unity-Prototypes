using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovementScript : MonoBehaviour
{
    Vector2 startPosition;

    void Start()
    {
        startPosition = gameObject.transform.position;
    }

    void Update()
    {
        Vector2 newPos = startPosition;
        newPos.y += Mathf.Sin(Time.realtimeSinceStartup);
        gameObject.transform.position = newPos;
    }
}
