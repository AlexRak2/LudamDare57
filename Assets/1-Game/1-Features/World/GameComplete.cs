using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class GameComplete : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameComplete"))
        {
            RenderSettings.skybox.DOColor(new Color(0.2971698f, 0.6872154f, 1f, 1f), 0.5f);
            StartCoroutine(ChangeFogDensity());
            StartCoroutine(PostProcChanger.instance.Change());
        }
    }

    private IEnumerator ChangeFogDensity()
    {
        while (RenderSettings.fogDensity > 0)
        {
            RenderSettings.fogDensity -= 0.2f * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
