using UnityEngine;
using Game.Input;

namespace LD57.UI
{
[RequireComponent(typeof(Canvas))]
public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;
    [SerializeField] private LD57Button closeButton;
    Canvas canvas;
    public bool IsSettingsCanvasActive => canvas.enabled;
    
    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        closeButton.Button.onClick.AddListener(CloseSettingsCanvas);
    }
    private void Start()
    {
        InputManager.SettingsActionTriggered += ToggleSettingsCanvas;
    }
    private void ToggleSettingsCanvas()
    {
        canvas.enabled = !canvas.enabled;
        Cursor.lockState = canvas.enabled ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = canvas.enabled;
    }
    private void CloseSettingsCanvas()
    {
        canvas.enabled = false;
    }
    private void OnDestroy()
    {
        InputManager.SettingsActionTriggered -= ToggleSettingsCanvas;
    }
}
}
