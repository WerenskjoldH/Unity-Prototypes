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

        RaycastHit2D[] hitObjects = Physics2D.CircleCastAll(transform.position, explosionRadius, Vector2.one);
        foreach (RaycastHit2D hit in hitObjects)
        {
            Debug.Log(hit.collider.name);
            if(hit.collider.gameObject.CompareTag("WorldObject"))
            {
                hit.collider.gameObject.GetComponent<WorldObjectInterface>().Hit();
            }
        }
    }

    void Update()
    {
        if (!IsAnimationPlaying())
            Destroy(gameObject);


    }
}
