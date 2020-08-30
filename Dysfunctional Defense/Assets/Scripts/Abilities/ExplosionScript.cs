using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    public float explosionRadius { get; set; } = 0.25f;

    Animator animator;

    bool IsAnimationPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    void Start()
    {
        animator = GetComponent<Animator>();

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D hit in hitObjects)
        {
            if(hit.gameObject.CompareTag("WorldObject"))
            {
                hit.gameObject.GetComponent<WorldObjectInterface>().Hit();
            }
        }
    }

    void Update()
    {
        if (!IsAnimationPlaying())
            Destroy(gameObject);


    }
}
