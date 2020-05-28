using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitZoneScript : MonoBehaviour
{
    [SerializeField] LevelManagerScript levelManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        levelManager.TriggerExitZoneReached();
    }
}
