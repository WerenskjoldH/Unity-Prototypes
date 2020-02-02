using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSway : MonoBehaviour
{
    Vector3 defaultLocalPosition;
    void Start()
    {
        defaultLocalPosition = transform.localPosition;
    }

    void Update()
    {
        Vector3 localPos = transform.localPosition;
        localPos.y = defaultLocalPosition.y + 0.3f * Mathf.Sin(Time.timeSinceLevelLoad) + 0.1f * Mathf.Sin(Time.timeSinceLevelLoad + Mathf.PI);
        transform.localPosition = localPos;
    }
}
