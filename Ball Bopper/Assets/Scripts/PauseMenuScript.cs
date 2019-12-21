using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{

    public static bool isPaused = true;

    public GameObject pauseMenuUI = null;

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    void Start()
    {
        PauseGame();
    }

    // Update is called once per frame
    void Update()
    {
        // This is for debugging
        // A button or so will need to be added
        // for mobile pausing
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
                Debug.Log("Game Resumed");
            }
            else
            {
                PauseGame();
                Debug.Log("Game Resumed");
            }

            Debug.Log("Escape Pressed");
        }
    }
}
