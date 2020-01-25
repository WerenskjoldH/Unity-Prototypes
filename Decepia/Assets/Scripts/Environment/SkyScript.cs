using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyScript : MonoBehaviour
{
    public Transform playerPosition;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playerPosition.position.x, transform.position.y, playerPosition.position.z);
    }
}
