using UnityEngine;


namespace LD57.UI
{
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private LD57Button startButton;
    [SerializeField] private LD57Button settingsButton;
    [SerializeField] private LD57CanvasGroup mainMenuCanvasGroup;
    void Start()
    {
        startButton.Button.onClick.AddListener(OnStartButtonClicked);
        settingsButton.Button.onClick.AddListener(OnSettingsButtonClicked);
        mainMenuCanvasGroup.CGDisable();
        GameManager.Instance.SettingsManager.OnSettingsCanvasToggle += OnSettingsCanvasToggle;
        GameManager.Instance.OnFadeComplete += OnFadingCanvasComplete;
    }

    private void OnStartButtonClicked()
    {
        StartCoroutine(GameManager.Instance.FadeCanvas(true));

    }
    private void OnSettingsButtonClicked()
    {
        mainMenuCanvasGroup.CGDisable();
        GameManager.Instance.SettingsManager.ToggleSettingsCanvas();
    }
    private void OnSettingsCanvasToggle(bool isActive)
    {
        if (!isActive) {
            LoadMainMenu();
        }
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnFadeComplete -= OnFadingCanvasComplete;
        GameManager.Instance.SettingsManager.OnSettingsCanvasToggle -= OnSettingsCanvasToggle;
    }
    private void LoadMainMenu()
    {
        mainMenuCanvasGroup.FadeIn(1f, true, true);
        Cursor.lockState =  CursorLockMode.None;
        Cursor.visible = true;
    }
    private void OnFadingCanvasComplete(bool isActive)
    {
        if (!isActive) {
            mainMenuCanvasGroup.FadeIn(1f, true, true);
        } else {
            GameManager.Instance.LoadLevel1();
        }
    }
}
}