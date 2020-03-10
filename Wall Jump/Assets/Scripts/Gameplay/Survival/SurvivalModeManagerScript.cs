﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalModeManagerScript : MonoBehaviour
{
    public GameObject startGameText;

    public PlayerControllerScript playerControllerScript;

    // Contains SurvivalModeWallScripts
    public ArrayList environmentObjects = new ArrayList();

    public List<GameObject> spawnablePrefabs = new List<GameObject>();

    public float scrollSpeed = 5.0f;
    public float spawnDistance = 12.0f;
    public Vector2 cullingDistanceFromCenter = new Vector2(12.0f, 8.0f);
    public Vector2 playerKillBoundsFromCenter = new Vector2(12.0f, 8.0f);

    int removedObject = 0;

    bool gameStart = false;

    private bool GameObjectCulling(GameObject gameObject, Vector2 objectPos)
    {
        // This code is to be removed once the object spawning system is added
        if (objectPos.x < -1 * cullingDistanceFromCenter.x || objectPos.y < -1 * cullingDistanceFromCenter.y)
        {
            Destroy(gameObject);
            removedObject++;
            return true;
        }

        return false;
    }

    private void GameObjectSpawning()
    {

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
            
            // World Object Updating
            foreach (GameObject o in environmentObjects)
            {
                Vector2 t = o.transform.position;
                t.x -= scrollSpeed * Time.deltaTime;

                if (GameObjectCulling(o, t))
                    continue;

                GameObjectSpawning();
                
                o.transform.position = t;
            }

            // Player Object Updating
            // Check for game over
            if(playerControllerScript.gameObject.transform.position.x < -1.0f * playerKillBoundsFromCenter.x || playerControllerScript.gameObject.transform.position.y < -1.0f * playerKillBoundsFromCenter.y)
            {
                // Destroy Player and Trigger Gameover
                Debug.Log("Game Over");
            }
        }
        
    }
}
