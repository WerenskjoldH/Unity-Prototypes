using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[ExecuteInEditMode]
public class CameraFollowScript : MonoBehaviour
{
    [SerializeField] GameObject TargetBody;
    [SerializeField] Vector3 bodyOffset;

    [SerializeField] bool updateInEditor = true;

    public void Awake()
    {
        if (Application.isPlaying)
            updateInEditor = true;
    }

    public void Update()
    {
        if (updateInEditor)
            gameObject.transform.position = TargetBody.transform.position + bodyOffset;
        else
            gameObject.transform.position = TargetBody.transform.position;

    }
}
