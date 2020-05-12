using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathOnContactScript : MonoBehaviour
{
    [SerializeField] bool canKillPlayer = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (!canKillPlayer)
            return;

        if (collision.gameObject.tag == "Player")
            collision.gameObject.GetComponent<PlayerControllerScript>().SetPlayerAlive(false);
    }
}
