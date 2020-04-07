using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidableScript : MonoBehaviour
{
    Mesh mesh;

    [SerializeField]
    float weight = 0.0f;
    [SerializeField]
    float sizeIncrease = 0.0f;

    Vector3 originalScale;

    public float GetWeight()
    {
        return weight;
    }

    private void Start()
    {
        originalScale = transform.localScale;
    }
}
