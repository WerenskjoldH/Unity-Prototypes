using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidableScript : MonoBehaviour
{
    public float sizeIncrease = 0.01f;

    Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public void RetainSize(float parentScale)
    {
        Vector3 rescale = originalScale / parentScale;

        transform.localScale = rescale;
    }
}
