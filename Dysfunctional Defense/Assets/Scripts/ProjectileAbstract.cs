using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileAbstract : MonoBehaviour, WorldObjectInterface
{
    [SerializeField]
    PlayerScript playerScript;

    public void Hit()
    {
        // ... Do what ever needs to be done on hit


        // One hit should typically destroy a building
        OnProjectileDestroy();
    }

    protected abstract void OnProjectileDestroy();
}
