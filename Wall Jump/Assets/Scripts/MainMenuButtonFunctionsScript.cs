using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonFunctionsScript : MonoBehaviour
{
    public SceneTransitionerScript transitionerScript;

    public void LevelSelectButton(string sceneName)
    {
        transitionerScript.LoadLevel(sceneName);
    }

    public void QuitButton()
    {
        Debug.LogWarning("Game Closing...");
        Application.Quit();
    }

}
