using System;
using System.Collections;
using System.Collections.Generic;
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
    int movementDirection;

    [SerializeField]
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

    void Start()
    {
        playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>();
        towerBase = playerControllerScript.towerBase;

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
        position.x += movementSpeed * Time.deltaTime;
        position.y += bobHeight * (float)Math.Sin(bobSpeed * Time.timeSinceLevelLoad);
        transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
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
        Destroy(gameObject);
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
