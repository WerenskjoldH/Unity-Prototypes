using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerScript : MonoBehaviour
{
    PlayerControllerScript playerScript;
    LevelStartPointScript levelStart;

    void Awake()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>();
        levelStart = GameObject.FindGameObjectWithTag("StartPoint").GetComponent<LevelStartPointScript>();
    }

    void Start()
    {
        MovePlayerToStart();    
    }

    void Update()
    {
        HandlePlayerDeath();
    }

    void MovePlayerToStart()
    {
        levelStart.MovePlayerToSpawn();
    }

    void HandlePlayerDeath()
    {
        if (playerScript.GetPlayerAlive())
            return;

        // If the player is dead we move him to the start point
        MovePlayerToStart();
    }
}
