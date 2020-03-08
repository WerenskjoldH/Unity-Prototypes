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

    bool gameStart = false;

    void Update()
    {
        if(!gameStart)
        {
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                gameStart = true;
                startGameText.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject o in environmentObjects)
            {
                Vector2 t = o.transform.position;
                t.y -= scrollSpeed * Time.deltaTime;

                if (t.y < -7.0f)
                    t.y = -1 * t.y;

                o.transform.position = t;
            }
        }
        
    }
}
