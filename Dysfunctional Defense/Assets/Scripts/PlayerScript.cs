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

    [SerializeField]
    GameObject missleObject;

    void Start()
    {
        
    }


    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            abilitySelector.GetCurrentAbility().InvokeAbility();
        }

        // This is for testing purposes
        if(Input.GetMouseButtonDown(1))
        {
            MissileScript missileObj = Instantiate(missleObject, new Vector3(0, 2), Quaternion.Euler(0.0f, 0.0f, UnityEngine.Random.Range(0.0f, 360.0f))).GetComponent<MissileScript>();
            missileObj.targetDestination = cityGameObjects[Random.Range(0, 3)].transform.position;
        }
    }
}
