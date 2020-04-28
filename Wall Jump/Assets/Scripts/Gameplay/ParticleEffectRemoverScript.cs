using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectRemoverScript : MonoBehaviour
{
    float timeBeforeDestroy = 0;
    private void Awake()
    {
        timeBeforeDestroy = GetComponent<ParticleSystem>().main.startLifetime.constantMax;
    }

    void Update()
    {
        if (timeBeforeDestroy <= 0)
            Destroy(gameObject);

        timeBeforeDestroy -= Time.deltaTime;
    }
}
