using UnityEngine;

public class LevelManagerScript : MonoBehaviour
{
    PlayerControllerScript playerScript;
    [SerializeField] LevelSpawnPointScript levelStart;

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
