using System.Collections;
using Game.Sounds;
using UnityEngine;

namespace LD57.Echo
{
public class EnemyEcho : InvisibleObject
{
    [SerializeField] private ParticleSystem echoParticleSystem;
    [SerializeField] private AudioClip echoSound;
    public override void EmitEcho()
    {
        base.EmitEcho();
        echoParticleSystem.Play();
        SoundManager.PlayWorld(echoSound, transform.position, 0.4f, 1f, false);
    }
}
}