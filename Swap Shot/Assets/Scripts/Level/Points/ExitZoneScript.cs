using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitZoneScript : MonoBehaviour
{
    [SerializeField] LevelManagerScript levelManager;

    // When the exit zone is triggered, we load the next level immediately
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        levelManager.LoadNextLevel();
    }
}
