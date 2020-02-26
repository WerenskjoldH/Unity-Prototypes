using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonFunctionsScript : MonoBehaviour
{
    public string PlayButtonSceneName;

    public void PlayButton()
    {
        SceneManager.LoadScene(PlayButtonSceneName);
    }

}
