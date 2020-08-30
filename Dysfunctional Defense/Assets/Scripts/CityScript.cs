using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityScript : BuildingAbstract
{

    public CityScript() : base(BuildingType.CITY)
    {

    }

    protected override void OnBuildingDestroy()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
