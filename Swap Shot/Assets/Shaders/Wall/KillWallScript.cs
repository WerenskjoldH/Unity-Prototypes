﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class KillWallScript : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Transform spherePoint;
    [SerializeField] float radius;

    void Start()
    {

    }

    void Update()
    {
        meshRenderer.material.SetFloat("Vector1_C5839ECE", radius);
        meshRenderer.material.SetVector("Vector3_CE91E199", spherePoint.position);
    }
}