using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectorScript : MonoBehaviour
{
    private bool isPaused = true;
    Vector3 startingPosition;

    [SerializeField]
    float boundsDistance = 0;
    [SerializeField]
    float speedModifier = 1.0f;

    [SerializeField]
    int numberOfAbilities = 5;

    float currentSelectionIndex = 0;

    void TogglePause()
    {
        isPaused = !isPaused;
    }

    public int GetCurrentAbilityIndex()
    {
        return (int)math.ceil(numberOfAbilities * (0.5f * (currentSelectionIndex + 1)));
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
