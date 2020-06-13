using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UmbrellaAttackerScript : MonoBehaviour, AttackerInterface
{
    [SerializeField]
    Animator umbrellaAttackerAnimator;
    [SerializeField]
    ParticleSystem umbrellaDeathParticle;

    [SerializeField]
    PlayerControllerScript playerControllerScript;
    GameObject towerBase;

    [SerializeField]
    float divebombSpeed;
    float attackRange;
    [SerializeField]
    float movementSpeedMin;
    [SerializeField]
    float movementSpeedMax;
    float movementSpeed;


    [SerializeField]
    float bobHeight;
    [SerializeField]
    float bobSpeed;
    float bobOffset;

    bool attacking = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "TowerPart")
        {
            DamageTower();
        }
        else if (collision.gameObject.tag != "Attacker")
            DestroySelf();

    }

    void Start()
    {
        playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>();
        towerBase = playerControllerScript.towerBase;

        attackRange = towerBase.transform.position.x + towerBase.GetComponent<BoxCollider2D>().bounds.extents.x;

        bobOffset = Time.frameCount;

        // Spawned on left or right side of screen
        if (gameObject.transform.position.x < 0)
        {
            movementSpeed = UnityEngine.Random.Range(movementSpeedMin, movementSpeedMax);
        }
        else
        {
            movementSpeed = -1.0f * UnityEngine.Random.Range(movementSpeedMin, movementSpeedMax);
        }
    }

    void Movement()
    {
        Vector3 position = transform.position;
        if (!attacking)
        {
            position.x += movementSpeed * Time.deltaTime;
            position.y += bobHeight * Mathf.Sin(bobSpeed * Time.timeSinceLevelLoad + bobOffset);
        }
        else
        {
            position.y -= divebombSpeed * Time.deltaTime;
        }
        transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(gameObject.transform.position.x - towerBase.transform.position.x) <= attackRange)
        {
            attacking = true;
            StartAttackingAnimation();
        }

        Movement();
    }

    public void Hit()
    {
        // Do all the death stuff
        Destroy(gameObject);
        Instantiate(umbrellaDeathParticle, transform.position, Quaternion.identity).Play();
    }

    // Called if the attacker makes it to the tower
    void DamageTower()
    {
        Debug.Log("Tower Attacked!");
        DestroySelf();
    }

    void DestroySelf()
    {
        Destroy(gameObject);
        Instantiate(umbrellaDeathParticle, transform.position, Quaternion.identity).Play();
    }

    void StartFloatingAnimation()
    {
        umbrellaAttackerAnimator.SetTrigger("floating");
    }

    void StartAttackingAnimation()
    {
        umbrellaAttackerAnimator.SetTrigger("attacking");
    }
}
