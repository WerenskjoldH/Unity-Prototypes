using UnityEngine;

public class LevelManagerScript : MonoBehaviour
{
    PlayerControllerScript playerScript;
    [SerializeField] LevelSpawnPointScript levelStart;

    bool levelTimerRunning = false;
    float levelTime = 0;

    #region Getters & Setters

    float GetLevelTime()
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

        if (levelTimerRunning)
            UpdateLevelTimer();
    }

    void Update()
    {
        HandlePlayerDeath();

        if (levelTimerRunning)
            UpdateLevelTimer();
    }

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

    void StartLevelTimer()
    {
        levelTimerRunning = true;
    }

    void UpdateLevelTimer()
    {
        // Just accumulates total time the player has been running through the level
        levelTime += Time.deltaTime;
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
}

