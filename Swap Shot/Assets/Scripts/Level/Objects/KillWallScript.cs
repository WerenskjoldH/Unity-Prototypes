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


    // To ensure the player never feels cheated, by being unable to control where their appendages are, we only check if the player
    //  has missed the hole in the wall via the exact point of the camera
    // Deaths related to legs hitting outside of the hole feel like you have been cheated,
    //  since in real life we can pull them up or adjust
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameObject player = other.gameObject;
            CinemachineVirtualCamera playerCamera = player.GetComponent<PlayerControllerScript>().GetPlayerVirtualCamera();
            if (Mathf.Abs((playerCamera.transform.position - spherePoint.position).magnitude) >= radius)
                player.GetComponent<PlayerControllerScript>().TriggerPlayerDeath();
        }
    }

    void Update()
    {
        // Radius could be be made a bit more static of a variable since on average it doesn't change that often
        // However, with current scope and the limited amount of information that must be synchronized between host and device
        //  it's okay
        // In production, probably not
        meshRenderer.material.SetFloat("Vector1_C5839ECE", radius);
        meshRenderer.material.SetVector("Vector3_CE91E199", spherePoint.position);
    }
}
