using System;
using System.Collections;
using UnityEngine;
using Unity.Mathematics;
using Game.Input;
using Game.UI;
using LD57.Echo;

namespace LD57.Player
{
public class PlayerInventoryManager : MonoBehaviour
{
   [SerializeField] private int maxEchoCount = 11;
   [SerializeField] private int currentEchoCount = 0;
   
   [SerializeField] private float _cooldownTime = 2f;
   [SerializeField] private float _cooldownTillRegenerate = 2f;
   [SerializeField] private float _regeneratePerSecond = 1.5f;
   float _currentCooldownTime = 0f;
    private void Start()
    {
        currentEchoCount = maxEchoCount;
        InputManager.EchoActionTriggered += OnEchoActionTriggered;
        PlayerStats.OnRespawn += ResetEchos;
        
        PlayerHud.Instance.UpdateEchoCount(new int2(maxEchoCount, maxEchoCount));
    }

    private void Update()
    {
        if(_currentCooldownTime > 0)
            _currentCooldownTime -= Time.deltaTime;
    }

    private void ResetEchos()
    {
        currentEchoCount = maxEchoCount;
        PlayerHud.Instance.UpdateEchoCount(new int2(currentEchoCount, maxEchoCount));
    }

    private void OnEchoActionTriggered()
    {
        if(_currentCooldownTime > 0) return;
        
        
        if (currentEchoCount <= 0) {
            Debug.Log("No echo left!");
            //play fail sfx, flash text red
            return;
        }
        
        StopAllCoroutines();

        currentEchoCount--;
        EchoScanner.Instance.EmitEcho();
        PlayerHud.Instance.UpdateEchoCount(new int2(currentEchoCount, maxEchoCount));
        
        StartCoroutine(RegenerateEchos());
        _currentCooldownTime = _cooldownTime;

    }

    IEnumerator RegenerateEchos()
    {
        yield return new WaitForSeconds(_cooldownTillRegenerate);

        while (currentEchoCount < maxEchoCount)
        {
            currentEchoCount++;
            PlayerHud.Instance.UpdateEchoCount(new int2(currentEchoCount, maxEchoCount));
            yield return new WaitForSeconds(_regeneratePerSecond);
        }
    }

    private void OnDestroy()
    {
        InputManager.EchoActionTriggered -= OnEchoActionTriggered;
    }
}
}
