﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/ExplosionAbility")]
class ExplosionAbility : AbilityAbstract
{
    [SerializeField]
    GameObject explosionObject;

    [SerializeField]
    float explosionRadius = 0.5f;

    ExplosionAbility()
    {
        // Empty For Now
    }

    public override void InvokeAbility()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        ExplosionScript explosionScript = Instantiate(explosionObject, mousePos, Quaternion.Euler(0.0f, 0.0f, UnityEngine.Random.Range(0.0f, 360.0f))).GetComponent<ExplosionScript>();
        explosionScript.explosionRadius = explosionRadius;
    }
}
