using System;
using UnityEngine;
using System.Collections;
using UnityEditor;

namespace LD57.Echo
{
public abstract class InvisibleObject : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    Material invisibleMaterial;
    private readonly float startMaterialTransparency = 0f;
    private readonly float endMaterialTransparency = 1.01f;

    private void OnValidate()
    {
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
            EditorUtility.SetDirty(this);
        }
    }

    protected virtual void Start()
    {
        invisibleMaterial = meshRenderer.material;
        invisibleMaterial.SetFloat("_Transparency", endMaterialTransparency);
        meshRenderer.sharedMaterial = invisibleMaterial;
    }
    public virtual void EmitEcho()
    {
        StartCoroutine(FadeOutMaterial());
    }
    private IEnumerator FadeOutMaterial()
    {
        invisibleMaterial.SetFloat("_Transparency", startMaterialTransparency);
        yield return new WaitForSeconds(1f);
        float fadeDuration = 1f;
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            invisibleMaterial.SetFloat("_Transparency", Mathf.Lerp(startMaterialTransparency, endMaterialTransparency, t));
            yield return null;
        }
        invisibleMaterial.SetFloat("_Transparency", endMaterialTransparency);
    }
}
}