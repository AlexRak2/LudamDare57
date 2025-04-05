using UnityEngine;
using Game.Input;

namespace LD57.Echo
{
public class EchoScanner : MonoBehaviour
{
    [SerializeField] private ParticleSystem echoParticleSystem;
    private void OnEnable()
    {
        InputManager.EchoActionTriggered += OnEchoActionTriggered;
    }
    private void OnEchoActionTriggered()
    {
        Debug.Log("Echo action triggered!");
        echoParticleSystem.Play();
    }
    private void OnDestroy()
    {
        InputManager.EchoActionTriggered -= OnEchoActionTriggered;
    }
}
}
