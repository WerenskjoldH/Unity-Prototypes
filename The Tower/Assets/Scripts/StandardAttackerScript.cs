using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardAttackerScript : MonoBehaviour, AttackerInterface
{
    [SerializeField]
    Animator standardAttackerAnimator;
    [SerializeField]
    ParticleSystem standardAttackerDeathParticle;

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

    void Start()
    {
        playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>();
        towerBase = playerControllerScript.towerBase;

        // Spawned on left or right side of screen
        if (gameObject.transform.position.x < 0)
        {
            movementSpeed = Random.Range(movementSpeedMin, movementSpeedMax);
        }
        else
        {
            movementSpeed = -1.0f * Random.Range(movementSpeedMin, movementSpeedMax);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "TowerBase")
        {
            Debug.Log("Tower Attacked!");
            DamageTower();
        }
    }

    void Movement()
    {
        StartWalkingAnimation();
        Vector3 position = transform.position;
        position.x += movementSpeed * Time.deltaTime;
        transform.position = position;
    }

    void Update()
    {
        Movement();
    }

    // Called if the player's beam destroys this attacker
    public void Hit()
    {
        // Do all the death stuff
        Destroy(gameObject);
        Instantiate(standardAttackerDeathParticle, transform.position, Quaternion.identity).Play();
    }

    // Called if the attacker makes it to the tower
    void DamageTower()
    {
        Destroy(gameObject);
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
