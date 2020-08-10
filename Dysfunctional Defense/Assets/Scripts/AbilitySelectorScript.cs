using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySelectorScript : MonoBehaviour
{
    private bool isPaused = true;
    Vector3 startingPosition;

    [SerializeField]
    float boundsDistance = 0;
    [SerializeField]
    float speedModifier = 1.0f;

    // To-Do: This should be scalable in the future
    [SerializeField]
    static int numberOfAbilities = 5;

    // As of now this must be manually kept to the number of abilities
    [SerializeField]
    List<AbilityAbstract> abilities;

    float currentSelectionIndex = 0;

    void TogglePause()
    {
        isPaused = !isPaused;
    }

    int GetCurrentAbilityIndex()
    {
        return (int)math.ceil(numberOfAbilities * (0.5f * (currentSelectionIndex + 1))) - 1;
    }

    public AbilityAbstract GetCurrentAbility()
    {
        Debug.Log(GetCurrentAbilityIndex());
        return abilities[GetCurrentAbilityIndex()];
    }

    void Start()
    {
        startingPosition = transform.position;

        TogglePause();
    }

    void Update()
    {
        if (isPaused)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
            Debug.Log(GetCurrentAbilityIndex());

        Vector3 position = transform.position;
        currentSelectionIndex = Mathf.Sin(speedModifier * Time.time);
        position.x = startingPosition.x + (boundsDistance * currentSelectionIndex);
        transform.position = position;
    }
}
