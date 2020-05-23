using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public Animator animator;
    // This is a bad approach
    public float couroutineWaitTime = 1f;

    public void LoadLevel(string name)
    {
        StartCoroutine(LoadLevelCouroutine(name));
    }

    IEnumerator LoadLevelCouroutine(string name)
    {
        animator.SetTrigger("End");

        yield return new WaitForSeconds(couroutineWaitTime);

        SceneManager.LoadScene(name);
    }
}
