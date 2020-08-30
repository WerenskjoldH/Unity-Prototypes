using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AbilitySelectorScript : MonoBehaviour
{
    private bool isPaused = true;
    Vector3 startingPosition;

    [SerializeField]
    float boundsDistance = 0;
    [SerializeField]
    float speedModifier = 1.0f;

    int numberOfAbilities = 0;

    [SerializeField]
    GameObject uiPanel;
    [SerializeField]
    UnityEngine.UI.Image uiIcon;
    [SerializeField]
    UnityEngine.UI.Image uiDivider;

    // As of now this must be manually kept to the number of abilities
    [SerializeField]
    List<AbilityAbstract> abilities;

    List<UnityEngine.UI.Image> uiAbilityIcons;
    List<UnityEngine.UI.Image> uiDividers;

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
        return abilities[GetCurrentAbilityIndex()];
    }

    // If abilities are changed, this must be called to refresh icons and placements
    void BuildUI()
    {
        float doubleBoundsDistance = 2.0f * boundsDistance;

        float dividerSpacing = doubleBoundsDistance * (1.0f/(numberOfAbilities));
        float dividerStartingPos = -boundsDistance + dividerSpacing;

        //float iconSpacing = doubleBoundsDistance * (1.0f/(numberOfAbilities+1));
        float iconStartingPos = -boundsDistance + dividerSpacing - (dividerSpacing/2.0f); 

        Transform panelTransform = uiPanel.transform;

        // Place Icons
        for (int i = 0; i < numberOfAbilities; i++)
        {
            GameObject uiGameObject = Instantiate<UnityEngine.UI.Image>(uiIcon, panelTransform).gameObject;
            RectTransform rectTran = uiGameObject.GetComponent<RectTransform>();
            rectTran.anchoredPosition = new Vector2(iconStartingPos + (i * dividerSpacing), 0);
            uiGameObject.GetComponent<UnityEngine.UI.Image>().sprite = abilities[i].abilitySprite;
        }

        // Place Dividers
        for (int i = 0; i < numberOfAbilities-1; i++)
        {
            RectTransform rectTran = Instantiate<UnityEngine.UI.Image>(uiDivider, panelTransform).GetComponent<RectTransform>();
            rectTran.anchoredPosition = new Vector2(dividerStartingPos + (i * dividerSpacing), 0);
        }

        transform.SetAsLastSibling();
    }

    void Start()
    {
        startingPosition = transform.position;
        numberOfAbilities = abilities.Count;

        BuildUI();

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
