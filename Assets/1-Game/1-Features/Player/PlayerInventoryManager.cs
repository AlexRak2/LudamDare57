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

    private void Start()
    {
        currentEchoCount = maxEchoCount;
        InputManager.EchoActionTriggered += OnEchoActionTriggered;
        PlayerHud.Instance.UpdateEchoCount(new int2(maxEchoCount, maxEchoCount));
    }
    private void OnEchoActionTriggered()
    {
        if (currentEchoCount <= 0) {
            Debug.Log("No echo left!");
            //play fail sfx, flash text red
            return;
        }

        currentEchoCount--;
        EchoScanner.Instance.EmitEcho();
        PlayerHud.Instance.UpdateEchoCount(new int2(currentEchoCount, maxEchoCount));
    }
    private void OnDestroy()
    {
        InputManager.EchoActionTriggered -= OnEchoActionTriggered;
    }
}
}
