using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

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
        while (volume.weight < 0.99f)
        {
            volume.weight += 0.3f * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
