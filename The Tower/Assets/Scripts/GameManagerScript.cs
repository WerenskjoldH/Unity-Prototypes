using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum SpawnerType
{
    GROUND,
    AIR
}

[System.Serializable]
struct Spawner
{
    public GameObject spawner;
    public SpawnerType spawnerType;
    // This is to be used for enabling the spawner after a set amount of game time has passed
    //  Allows for progressively more difficult enemies to spawn
    //public float enableTime;
}

public class GameManagerScript : MonoBehaviour
{
    #region Spawners & Enemies Variables
    [SerializeField]
    List<Spawner> spawnerList;

    // If more enemies are added, this would do better as a dictionary
    [SerializeField]
    List<GameObject> enemyList;
    #endregion

    #region Player Variables
    [SerializeField]
    PlayerControllerScript playerControllerScript;
    #endregion

    #region Spawning Variables
    [SerializeField]
    float maxSpawnTime = 5.0f;
    [SerializeField]
    float maxSpawnTimeReduction = 2.0f;
    // This is per-spawn
    [SerializeField]
    float timeReductionIncrement = 0.02f;
    [SerializeField]
    float minSpawnTime = 1.0f;
    // The randomly selected value between min and max to count up to
    float targetSpawnTime = 0.0f;
    float spawnTimeCtr = 0.0f;
    #endregion

    #region Pregame Variables
    bool startGame = false;

    [SerializeField]
    TMP_Text startGameText;
    #endregion

    void SpawnAttacker()
    {
        Spawner selectedSpawner = spawnerList[Random.Range(0, spawnerList.Count)];

        if(selectedSpawner.spawnerType == SpawnerType.GROUND)
            Instantiate(enemyList[0], selectedSpawner.spawner.transform.position, Quaternion.identity);
        else if(selectedSpawner.spawnerType == SpawnerType.AIR)
            Instantiate(enemyList[1], selectedSpawner.spawner.transform.position, Quaternion.identity);
    }


    void PreGameUpdate()
    {
        if(playerControllerScript.GetPlayerMouseScript().GetMouseDown())
        {
            startGame = true;
            startGameText.gameObject.SetActive(false);
        }
    }

    void GameUpdate()
    {
        /// Spawning
        if (spawnTimeCtr >= targetSpawnTime)
        {
            // Spawn
            SpawnAttacker();
            spawnTimeCtr = 0;
            targetSpawnTime = Random.Range(minSpawnTime, maxSpawnTime - maxSpawnTimeReduction);
            if (maxSpawnTime - maxSpawnTimeReduction >= minSpawnTime)
                maxSpawnTimeReduction += timeReductionIncrement;
        }

        spawnTimeCtr += Time.deltaTime;
    }

    void Update()
    {
        if (!startGame)
            PreGameUpdate();
        else
            GameUpdate();
    }
}