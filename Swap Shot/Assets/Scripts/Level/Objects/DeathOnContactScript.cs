using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathOnContactScript : MonoBehaviour
{
    [SerializeField] bool canKillPlayer = true;

    // This will kill the player on contact
    private void OnCollisionEnter(Collision collision)
    {
        if (!canKillPlayer)
            return;

        if (collision.gameObject.tag == "Player")
            collision.gameObject.GetComponent<PlayerControllerScript>().SetPlayerAlive(false);
    }
}
