using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalModeManagerScript : MonoBehaviour
{
    public GameObject startGameText;

    public PlayerControllerScript playerControllerScript;

    // Contains SurvivalModeWallScripts
    public ArrayList environmentObjects = new ArrayList();

    public float scrollSpeed = 5.0f;
    public Vector2 cullingDistanceFromCenter = new Vector2(12.0f, 12.0f);

    bool gameStart = false;

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

            foreach (GameObject o in environmentObjects)
            {
                Vector2 t = o.transform.position;
                t.x -= scrollSpeed * Time.deltaTime;

                // This code is to be removed once the object spawning system is added
                if (t.x < -1 * cullingDistanceFromCenter.x)
                    Destroy(o);
                //t.x = -1 * t.x;
                if (t.y < -1 * cullingDistanceFromCenter.y)
                    Destroy(o);
                    //t.y = -1 * t.y;

                o.transform.position = t;
            }
        }
        
    }
}
