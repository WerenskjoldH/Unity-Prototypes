using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/*
 *  To-Do:
 * 
 *      - Trail Renderer should be placed on a child and unparented on destruction so that the trail does not immediately disappear when the missile is detonated
 */


public class MissileScript : ProjectileAbstract
{
    [SerializeField]
    GameObject explosionObject;

    public Vector3 targetDestination { get; set; }
    Vector3 targetDirection;

    [SerializeField]
    float speed = 1.0f;
    [SerializeField]
    float explosionRadius = 0.1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("WorldObject"))
        {
            Detonate();
        }
    }

    void Start()
    {
        targetDirection = (targetDestination - transform.position).normalized;
    }

    void Update()
    {
        Vector3 position = transform.position;
        
        position += targetDirection * speed * Time.deltaTime;

        transform.position = position;

        transform.rotation = Quaternion.LookRotation(targetDirection);
    }

    protected override void Detonate()
    {
        ExplosionScript explosionScript = Instantiate(
            explosionObject, 
            transform.position, 
            Quaternion.identity
            ).GetComponent<ExplosionScript>();

        explosionScript.explosionRadius = explosionRadius;

        Destroy(gameObject);
    }
}
