using UnityEngine;

namespace LD57.Echo
{
public class EnemyEcho : MonoBehaviour
{
    [SerializeField] private ParticleSystem echoParticleSystem;

    public void EmitEcho()
    {
        echoParticleSystem.Play();
    }
}
}