using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    AbilitySelectorScript abilitySelector;

    [SerializeField]
    List<BuildingAbstract> cityGameObjects;

    void Start()
    {
        
    }


    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            abilitySelector.GetCurrentAbility().InvokeAbility();
        }
    }
}
