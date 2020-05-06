using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartPointScript : MonoBehaviour
{
    Transform playerTransform;
    static bool multiplesFound = false;

    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
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

    void Start()
    {
        DetectMultipleSpawnPoints();

        playerTransform.position = transform.position;
    }
}
