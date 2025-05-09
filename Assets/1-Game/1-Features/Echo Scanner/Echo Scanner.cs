using UnityEngine;
using Game.Input;
using System.Collections;
using Game.Sounds;

namespace LD57.Echo
{
public class EchoScanner : MonoBehaviour
{
    public static EchoScanner Instance;
    [SerializeField] private ParticleSystem echoParticleSystem;
    [SerializeField] private SphereCollider echoCollider;
    [SerializeField] private float echoInitialScale = 0.1f;
    [SerializeField] private float echoFinalScale = 1f;
    [SerializeField] private float echoDuration = 1f, dimDuration = 2f; 
    private Material particleMaterial;
    [SerializeField] private float initialFloat = 0.5f;
    [SerializeField] private AudioClip echoSound;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        // Get the material from the Particle System's renderer
        particleMaterial = echoParticleSystem.GetComponent<ParticleSystemRenderer>().material;
    }
    public void EmitEcho()
    {
        SoundManager.PlayWorld(echoSound, transform.position, 0.3f, 1f, false);
        echoParticleSystem.Play();
        StartCoroutine(ScaleCollider());
        StartCoroutine(DimParticleMaterial());
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
    private IEnumerator DimParticleMaterial()
    {
        float elapsedTime = 0f;
        float finalFloat = 0f;
        
        while (elapsedTime < dimDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / dimDuration);
            float currentFloat = Mathf.Lerp(initialFloat, finalFloat, t);
            particleMaterial.SetFloat("_IntersectionDepth", currentFloat);
            yield return null;
        }
        particleMaterial.SetFloat("_IntersectionDepth", finalFloat);
    }
}
}
