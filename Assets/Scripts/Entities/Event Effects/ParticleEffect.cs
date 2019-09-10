using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : EventEffect
{
    new private ParticleSystem particleSystem;

    protected override void Awake()
    {
        base.Awake();
        particleSystem = GetComponent<ParticleSystem>();
    }

    protected override void DoEffect()
    {
        particleSystem.Play();
    }
    protected override void UndoEffect()
    {
        particleSystem.Stop();
    }
}
