using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class SubmitFunction : UnityEvent { }

public class MenuButtonScript : MonoBehaviour
{
    [SerializeField] MenuControllerScript menuButtonController;
    [SerializeField] Animator animator;
    [SerializeField] AnimatorFunctionsScript animatorFunctions;
    [SerializeField] int thisIndex;
    public SubmitFunction submitMethod;

    void Update()
    {
        if (menuButtonController.index == thisIndex)
        {
            animator.SetBool("selected", true);
            if (Input.GetAxis("Submit") == 1)
            {
                animator.SetBool("pressed", true);
                if(submitMethod.GetPersistentEventCount() > 0)
                    submitMethod.Invoke();
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
}