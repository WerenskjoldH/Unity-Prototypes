using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionerScript : MonoBehaviour
{
    public Animator animator;
    public float couroutineWaitTime = 1f;

    public void LoadLevel(string name)
    {
        StartCoroutine(LoadLevelCouroutine(name));
    }

    IEnumerator LoadLevelCouroutine(string name)
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(couroutineWaitTime);

        SceneManager.LoadScene(name);
    }
}
