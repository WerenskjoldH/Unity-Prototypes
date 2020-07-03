using UnityEngine;

public class LevelManagerScript : MonoBehaviour
{
    PlayerControllerScript playerScript;
    [SerializeField] LevelSpawnPointScript levelStart;
    [SerializeField] SceneManagerScript sceneManager;
    [SerializeField] string nameOfNextLevel;

    bool levelTimerRunning = false;
    float levelTime = 0;

    #region Getters & Setters

    /*
        @return Gets the length of time that has passed since the player started moving
     */
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

    // Loads the next level
    // This is expected to be triggered by the ExitZoneScript
    public void LoadNextLevel()
    {
        sceneManager.LoadLevel(nameOfNextLevel);
    }

    #region Handle Player

    // This resets the level and moves the player back to the starting point
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

    // Handles the logic involved in keeping track of the timer and when it should run
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

    // Starts the level timer
    void StartLevelTimer()
    {
        levelTimerRunning = true;
    }

    // Resets the level timer AND disables it running until started again
    void ResetLevelTimer()
    {
        levelTimerRunning = false;
        levelTime = 0;
    }

    // Stops the timer only, does NOT disable it running
    void StopLevelTimer()
    {
        levelTimerRunning = false;
    }

    #endregion
}

