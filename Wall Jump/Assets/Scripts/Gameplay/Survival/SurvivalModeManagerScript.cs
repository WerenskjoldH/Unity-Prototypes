using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObstaclePrefab
{
    public int difficulty;
    public GameObject obstaclePrefab;
}

public class SurvivalModeManagerScript : MonoBehaviour
{
    public GameObject startGameText;

    public PlayerControllerScript playerControllerScript;

    // Contains SurvivalModeWallScripts
    public ArrayList environmentObjects = new ArrayList();

    public int currentDifficulty = 0;
    // For future reference, this SHOULD be put to a dictionary for quickly referencing difficulty to obstacle arrays
    public ObstaclePrefab[] spawnablePrefabs;

    public float scrollSpeed = 5.0f;
    public float spawnDistance = 12.0f;
    // Absolute vertical distance from y 
    public float spawnMidpointDistance = 5.0f;
    public Vector2 cullingDistanceFromCenter = new Vector2(12.0f, 8.0f);
    public Vector2 playerKillBoundsFromCenter = new Vector2(12.0f, 8.0f);

    public float timeBetweenSpawns = 10.0f;
    float timeBetweenSpawnsAcc = 0.0f;

    int removedObject = 0;

    bool gameStart = false;
    bool shouldSpawn = false;
    

    private bool GameObjectCulling(GameObject observedGameObject, Vector2 objectPos)
    {
        // This code is to be removed once the object spawning system is added
        if (objectPos.x < -1 * cullingDistanceFromCenter.x || objectPos.y < -1 * cullingDistanceFromCenter.y)
        {
            Destroy(observedGameObject);
            removedObject++;
            return true;
        }

        return false;
    }

    private void GameObjectSpawning()
    {
        /// Spawn Decision code
        if (!shouldSpawn)
        {
            timeBetweenSpawnsAcc += Time.deltaTime;
            if (timeBetweenSpawnsAcc >= timeBetweenSpawns)
                shouldSpawn = true;
        }
        else
        {

            /// Spawning Code
            // This is all to be changed
            Instantiate(spawnablePrefabs[0].obstaclePrefab, new Vector2(spawnDistance, Random.Range(-1 * spawnMidpointDistance, spawnMidpointDistance)), Quaternion.Euler(0, 0, 90));
            shouldSpawn = false;
            timeBetweenSpawnsAcc = 0;
        }
    }

    void Update()
    {
        if(!gameStart)
        {
            /// Pre-gameplay Code
            // Input starts the game and removes the start game text
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                gameStart = true;
                startGameText.SetActive(false);
            }
        }  
        else
        {
            /// Gameplay Code
            
            // Obstacle Updating
            foreach (GameObject o in environmentObjects)
            {
                Vector2 t = o.transform.position;
                t.x -= scrollSpeed * Time.deltaTime;

                if (GameObjectCulling(o, t))
                    continue;
                
                o.transform.position = t;
            }

            // Obstacle Spawning
            GameObjectSpawning();

            // Player Object Updating
            // Check for game over
            if (playerControllerScript.gameObject.transform.position.x < -1.0f * playerKillBoundsFromCenter.x || playerControllerScript.gameObject.transform.position.y < -1.0f * playerKillBoundsFromCenter.y)
            {
                // Destroy Player and Trigger Gameover
                Debug.Log("Game Over");
            }
        }
        
    }
}
