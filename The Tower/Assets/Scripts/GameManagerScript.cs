using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    // Could set this up as a dictionary if I add new attackers and spawner types
    [SerializeField]
    List<Spawner> spawnerList;

    [SerializeField]
    List<GameObject> enemyList;

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

    void SpawnAttacker()
    {
        Spawner selectedSpawner = spawnerList[Random.Range(0, spawnerList.Count)];
        // Default enemy
        Instantiate(enemyList[0], selectedSpawner.spawner.transform.position, Quaternion.identity);
    }

    void Start()
    {
    }

    void Update()
    {
        /// Spawning
        if(spawnTimeCtr >= targetSpawnTime)
        {
            // Spawn
            SpawnAttacker();
            spawnTimeCtr = 0;
            targetSpawnTime = Random.Range(minSpawnTime, maxSpawnTime - maxSpawnTimeReduction);
            if(maxSpawnTime - maxSpawnTimeReduction >= minSpawnTime)
                maxSpawnTimeReduction += timeReductionIncrement;
        }

        spawnTimeCtr += Time.deltaTime;
    }
}

public enum SpawnerType
{
    GROUND
}

[System.Serializable]
struct Spawner
{
    public GameObject spawner;
    public SpawnerType spawnerType;
}