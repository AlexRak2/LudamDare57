using System;
using System.Collections;
using UnityEngine;

public class GameComplete : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameComplete"))
        {
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
