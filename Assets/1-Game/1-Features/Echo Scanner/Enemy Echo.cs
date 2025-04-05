using System.Collections;
using UnityEngine;

namespace LD57.Echo
{
public class EnemyEcho : InvisibleObject
{
    [SerializeField] private ParticleSystem echoParticleSystem;
    public override void EmitEcho()
    {
        base.EmitEcho();
        echoParticleSystem.Play();
    }
}
}