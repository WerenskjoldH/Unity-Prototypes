using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonFunctionsScript : MonoBehaviour
{
    public string PlayButtonSceneName;
    public SceneTransitionerScript transitionerScript;

    public void PlayButton()
    {
        transitionerScript.LoadLevel(PlayButtonSceneName);
    }

    public void QuitButton()
    {
        Debug.LogWarning("Game Closing...");
        Application.Quit();
    }

}
