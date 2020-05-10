using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartPointScript : MonoBehaviour
{
    PlayerControllerScript playerScript;
    static bool multiplesFound = false;

    void Awake()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>();
    }

    void Start()
    {
        DetectMultipleSpawnPoints();
    }

    public void MovePlayerToSpawn()
    {
        playerScript.SetPosition(transform.position);
    }

    //Checks if the designer accidentally placed two spawn points
    void DetectMultipleSpawnPoints()
    {
        if (multiplesFound)
            return;

        int numberOfSpawnPoints = GameObject.FindGameObjectsWithTag("StartPoint").Length;

        if (numberOfSpawnPoints > 1)
        {
            Debug.LogError(numberOfSpawnPoints + " Start Points Found, Fix This Immediately");
            multiplesFound = true;
        }
    }
}
