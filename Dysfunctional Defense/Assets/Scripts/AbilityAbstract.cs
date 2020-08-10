using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbilityAbstract : ScriptableObject
{
    public string abilityName = "Ability Name Undefined";
    public float cooldownTime = 1.0f;
    public Sprite abilitySprite;

    public abstract void InvokeAbility();
}

