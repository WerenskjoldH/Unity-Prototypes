using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardAttackerScript : MonoBehaviour
{
    [SerializeField]
    Animator standardAttackerAnimator;

    [SerializeField]
    GameObject towerBase;

    [SerializeField]
    float attackRange;
    [SerializeField]
    float movementSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Hit()
    {
        // Do all the death stuff
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }

    void StartWalkingAnimation()
    {
        // The animation is already running
        if (standardAttackerAnimator.GetBool("walking"))
            return;

        standardAttackerAnimator.SetBool("walking", true);
        standardAttackerAnimator.SetBool("attacking", false);
    }

    void StartAttackingAnimation()
    {
        // The animation is already running
        if (standardAttackerAnimator.GetBool("attacking"))
            return;

        standardAttackerAnimator.SetBool("attacking", true);
        standardAttackerAnimator.SetBool("walking", false);
    }

    void StartIdleAnimation()
    {
        if (standardAttackerAnimator.GetBool("idle"))
            return;

        standardAttackerAnimator.SetBool("idle", true);
        standardAttackerAnimator.SetBool("walking", false);
        standardAttackerAnimator.SetBool("attacking", false);
    }
}
