using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidableScript : MonoBehaviour
{
    Mesh mesh;

    public float GetSize()
    {
        return mesh.bounds.size.magnitude * transform.localScale.magnitude;
    }

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }
}
