using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/ExplosionAbility")]
class ExplosionAbility : AbilityAbstract
{
    ExplosionAbility()
    {
        // Empty For Now
    }

    public override void InvokeAbility()
    {
        // Instantiate Explosion Game Object
        Debug.Log("Imagine an explosion now!");
        throw new NotImplementedException();
    }
}
