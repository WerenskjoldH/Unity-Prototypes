using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwap : MonoBehaviour
{
    public Camera playerCamera;
    public Light playerLight;
    public Camera monsterCamera;

    void Start()
    {
        playerCamera.enabled = true;
        playerCamera.GetComponent<AudioListener>().enabled = true;
        monsterCamera.enabled = false;
        monsterCamera.GetComponent<AudioListener>().enabled = false;
        playerLight.enabled = false;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playerCamera.enabled = false;
            playerCamera.GetComponent<AudioListener>().enabled = false;
            monsterCamera.enabled = true;
            monsterCamera.GetComponent<AudioListener>().enabled = true;
            playerLight.enabled = true;
        }
        
        if(Input.GetMouseButtonUp(0))
        {
            playerCamera.enabled = true;
            playerCamera.GetComponent<AudioListener>().enabled = true;
            monsterCamera.enabled = false;
            monsterCamera.GetComponent<AudioListener>().enabled = false;
            playerLight.enabled = false;
        }
    }
}
