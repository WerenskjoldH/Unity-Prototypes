using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidableScript : MonoBehaviour
{
    Mesh mesh;
    public AudioSource audioSource;

    public float GetSize()
    {
        return mesh.bounds.size.magnitude * transform.localScale.magnitude;
    }

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(0.75f, 1);
    }
}
