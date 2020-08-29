using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BuildingType
{
    CITY,
    SADSITE, // Surface to Air Defensive(SAD) Site
    UNKNOWN
}

public abstract class BuildingAbstract : MonoBehaviour
{
    public BuildingType buildingType { get; protected set; } = BuildingType.UNKNOWN;
    public bool isDestroyed { get; protected set; } = true;

    [SerializeField]
    PlayerScript playerScript;

    public BuildingAbstract(BuildingType _buildingType)
    {
        buildingType = _buildingType;
    }

    public void Hit()
    {
        // ... Do what ever needs to be done on hit


        // One hit should typically destroy a building
        OnDestroy();
    }

    protected abstract void OnDestroy();


}
