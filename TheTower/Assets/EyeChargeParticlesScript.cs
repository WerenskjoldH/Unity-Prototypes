using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeChargeParticlesScript : MonoBehaviour
{
    ParticleSystem chargeParticleSystem;

    private void Start()
    {
        chargeParticleSystem = GetComponent<ParticleSystem>();
    }

    public void PlayChargeEffect()
    {
        chargeParticleSystem.Play();
    }

    public void StopChargeEffect()
    {
        chargeParticleSystem.Clear();
    }
}
