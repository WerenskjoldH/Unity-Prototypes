using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class KillWallScript : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Transform spherePoint;
    [SerializeField] float radius;




    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);

        // For prototype purposes we are going to just do something a little janky
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Player Detected");
            GameObject player = other.gameObject;
            CinemachineVirtualCamera playerCamera = player.GetComponent<PlayerControllerScript>().GetPlayerVirtualCamera();
            if (Mathf.Abs((playerCamera.transform.position - spherePoint.position).magnitude) >= radius)
                player.GetComponent<PlayerControllerScript>().SetPlayerAlive(false);
        }
    }

    void Start()
    {

    }

    void Update()
    {
        meshRenderer.material.SetFloat("Vector1_C5839ECE", radius);
        meshRenderer.material.SetVector("Vector3_CE91E199", spherePoint.position);
    }
}
