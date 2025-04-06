using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcChanger : MonoBehaviour
{
    public static PostProcChanger instance;
    public Volume volume;
    
    private void Awake()
    {
        instance = this;
    }

    public IEnumerator Change()
    {
        while ((volume.profile.components[0] as ColorAdjustments).postExposure.value > 2)
        {
            (volume.profile.components[0] as ColorAdjustments).postExposure.value -= 2f * Time.fixedDeltaTime;
            volume.weight += 0.5f * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
