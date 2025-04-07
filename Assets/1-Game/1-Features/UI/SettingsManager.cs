using UnityEngine;
using Game.Input;
using UnityEngine.Rendering;
using System;
using Game.Sounds;
using Game.UI;

namespace LD57.UI
{
[RequireComponent(typeof(Canvas))]
public class SettingsManager : MonoBehaviour
{
    [SerializeField] private LD57Button closeButton, mainMenuButton, exitGameButton;
    [SerializeField] private GameObject settingsVolume;
    Canvas canvas;
    public bool IsSettingsCanvasActive => canvas.enabled;
    public Action<bool> OnSettingsCanvasToggle;
    bool isFadingToMainMenu = false;
    
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        closeButton.Button.onClick.AddListener(CloseSettingsCanvas);
        mainMenuButton.Button.onClick.AddListener(OnMainMenuButtonClicked);
        exitGameButton.Button.onClick.AddListener(OnExitGameButtonClicked);
        settingsVolume.SetActive(false);
    }
    private void Start()
    {
        InputManager.SettingsActionTriggered += ToggleSettingsCanvas;
        GameManager.Instance.OnFadeComplete += OnFadingCanvasComplete;
    }
    private void OnDestroy()
    {
        InputManager.SettingsActionTriggered -= ToggleSettingsCanvas;
        GameManager.Instance.OnFadeComplete -= OnFadingCanvasComplete;
    }
    public void ToggleSettingsCanvas()
    {
        if(PlayerHud.Instance && PlayerHud.IsDeathScreenOn) return;
        
        SoundManager.Instance.PlayUISFX(SFXData.ButtonClick);
        canvas.enabled = !canvas.enabled;
        Cursor.lockState = canvas.enabled ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = canvas.enabled;
        settingsVolume.SetActive(canvas.enabled);
        OnSettingsCanvasToggle?.Invoke(canvas.enabled);
    }
    private void CloseSettingsCanvas()
    {
        ToggleSettingsCanvas();
    }
    private void OnMainMenuButtonClicked()
    {
        ToggleSettingsCanvas();
        isFadingToMainMenu = true;
        StartCoroutine(GameManager.Instance.FadeCanvas(true));
    }
    private void OnExitGameButtonClicked()
    {
        Application.Quit();
    }
    private void OnFadingCanvasComplete(bool isActive)
    {
        if (isActive && isFadingToMainMenu) {
            isFadingToMainMenu = false;
            GameManager.Instance.LoadMainMenu();
        }
    }
}
}
