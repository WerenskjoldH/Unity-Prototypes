using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnCollisionEnter(Collision collision)
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
    }

    protected override void Detonate()
    {
        ExplosionScript explosionScript = Instantiate(
            explosionObject, 
            transform.position, 
            Quaternion.Euler(0.0f, 0.0f, UnityEngine.Random.Range(0.0f, 360.0f))
            ).GetComponent<ExplosionScript>();

        explosionScript.explosionRadius = explosionRadius;
    }
}
