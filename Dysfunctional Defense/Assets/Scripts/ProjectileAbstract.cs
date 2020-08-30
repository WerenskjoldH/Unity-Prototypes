using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileAbstract : MonoBehaviour, WorldObjectInterface
{
    public void Hit()
    {
        // ... Do what ever needs to be done on hit


        // One hit should normally detonate a projectile
        Detonate();
    }

    protected abstract void Detonate();
}
