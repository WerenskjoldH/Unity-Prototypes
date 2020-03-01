using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


[Serializable]
public class SubmitFunction : UnityEvent { }

public class MenuButtonScript : MonoBehaviour
{
    [SerializeField] MenuControllerScript menuButtonController;
    [SerializeField] Animator animator;
    [SerializeField] AnimatorFunctionsScript animatorFunctions;
    [SerializeField] int thisIndex;
    public SubmitFunction submitMethod;
    public bool mouseHovering = false;

    public int GetIndex()
    {
        return thisIndex;
    }

    void Update()
    {

        if (menuButtonController.index == thisIndex)
        {
            animator.SetBool("selected", true);
            if (Input.GetAxis("Submit") == 1 || (mouseHovering && Input.GetMouseButtonDown(0)))
            {
                animator.SetBool("pressed", true);
            }
            else if (animator.GetBool("pressed"))
            {
                animator.SetBool("pressed", false);
            }
        }
        else
        {
            animator.SetBool("selected", false);
        }
    }

    void InvokeSubmitMethod()
    {
        if (submitMethod.GetPersistentEventCount() > 0)
            submitMethod.Invoke();
    }
}