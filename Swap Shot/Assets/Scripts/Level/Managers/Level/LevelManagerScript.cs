﻿using UnityEngine;

public class LevelManagerScript : MonoBehaviour
{
    PlayerControllerScript playerScript;
    [SerializeField] LevelSpawnPointScript levelStart;
    [SerializeField] SceneManagerScript sceneManager;
    [SerializeField] string nameOfNextLevel;

    bool levelTimerRunning = false;
    float levelTime = 0;

    #region Getters & Setters

    public float GetLevelTime()
    {
        return levelTime;
    }

    #endregion

    void Awake()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>();
        if (levelStart == null)
            Debug.LogError("Level Start Not Set In Level Manager Object, Fix This Immediately");
    }

    void Start()
    {
        MovePlayerToStart();
    }

    void Update()
    {
        HandlePlayerDeath();

        HandleTimerUpdate();
    }

    public void TriggerExitZoneReached()
    {
        sceneManager.LoadLevel(nameOfNextLevel);
    }

    #region Handle Player

    void MovePlayerToStart()
    {
        ResetLevelTimer();
        levelStart.MovePlayerToSpawn();
    }

    void HandlePlayerDeath()
    {
        if (playerScript.GetPlayerAlive())
            return;

        // If the player is dead we move him to the start point
        MovePlayerToStart();
    }

    void HandleTimerUpdate()
    {
        if (levelTimerRunning)
        {
            // Just accumulates total time the player has been running through the level
            levelTime += Time.deltaTime;
        }
        else
        {
            if (playerScript.GetStartInput())
                StartLevelTimer();
        }
    }

    void StartLevelTimer()
    {
        levelTimerRunning = true;
    }

    void ResetLevelTimer()
    {
        levelTimerRunning = false;
        levelTime = 0;
    }

    void StopLevelTimer()
    {
        levelTimerRunning = false;
    }

    #endregion
}
