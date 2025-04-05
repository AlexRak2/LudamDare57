using UnityEngine;
using Game.Input;
using System.Collections;

namespace LD57.Echo
{
public class EchoScanner : MonoBehaviour
{
    [SerializeField] private ParticleSystem echoParticleSystem;
    [SerializeField] private SphereCollider echoCollider;
    [SerializeField] private float echoInitialScale = 0.1f;
    [SerializeField] private float echoFinalScale = 1f;
    [SerializeField] private float echoDuration = 1f; 
    private void OnEnable()
    {
        InputManager.EchoActionTriggered += OnEchoActionTriggered;
    }
    private void OnEchoActionTriggered()
    {
        echoParticleSystem.Play();
        StartCoroutine(ScaleCollider());
    }
    private void OnDestroy()
    {
        InputManager.EchoActionTriggered -= OnEchoActionTriggered;
    }
    private IEnumerator ScaleCollider()
    {
        float elapsedTime = 0f;
        Vector3 initialScale = new Vector3(echoInitialScale, echoInitialScale, echoInitialScale);
        Vector3 finalScale = new Vector3(echoFinalScale, echoFinalScale, echoFinalScale);
        echoCollider.transform.localScale = initialScale;
        echoCollider.enabled = true;
        while (elapsedTime < echoDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / echoDuration);
            echoCollider.transform.localScale = Vector3.Lerp(initialScale, finalScale, t);
            yield return null;
        }
        echoCollider.transform.localScale = finalScale;
        echoCollider.enabled = false;
    }
}
}
